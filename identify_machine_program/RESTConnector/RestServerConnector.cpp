#include <cpprest/http_client.h>
#include <cpprest/filestream.h>
#include <string>
#include <vector>
#include "IOConfig/IOConfig.h"
#include "RestServerConnector.h"

using namespace utility;              // Common utilities like string conversions
using namespace web;                  // Common features like URIs.
using namespace web::http;            // Common HTTP functionality
using namespace web::http::client;    // HTTP client features
using namespace concurrency::streams; // Asynchronous streams

RestServerConnector::RestServerConnector(){}
RestServerConnector::~RestServerConnector(){
    delete initUserDetails;
    delete openSSLCryptoUtil;
}

int RestServerConnector::sendData(std::string inputIndex, std::string inputCode) {
    IOConfig ioConfig = IOConfig();
    if(!ioConfig.doesFileExist(registrationFilePath)){
        return executeRequest(returnUserDTOAsJson(inputIndex,inputCode), registrationFilePath, registerEndpoint);    
    }else if(ioConfig.isSecretFileEmpty(registrationFilePath)){
        return executeRequest(returnUserDTOAsJson(inputIndex,inputCode), registrationFilePath, registerEndpoint);
    }
    // this should not happen
    return -1;
}

int RestServerConnector::executeRequest(json::value json_par, std::string filePath, std::string endpointName){
    static IOConfig ioConfig = IOConfig();
    static int returnedHttpCode;
    static std::string responseString;
    auto fileStream = std::make_shared<ostream>();

    pplx::task<void> requestTask =
        
        fstream::open_ostream(filePath)

            .then([=](ostream outFile) {
                *fileStream = outFile;

                // Create http_client to send the request.
                http_client client("http://localhost:8080");

                uri_builder builder(endpointName);
                //builder.append_query("q", "facebook");

                return client.request(methods::POST, builder.to_string(), json_par);
            })

            // Handle response headers arriving.
            .then([=](http_response response) {
                returnedHttpCode = response.status_code();
                responseString = response.extract_string().get();   

                if(ioConfig.trim(responseString).length()!=0){
                    //szyfruj
                    responseString = encryptAES256WithOpenSSL(responseString);

                    for(int i=0; i<responseString.length();i++){
                        fileStream->streambuf().putc(responseString[i]);
                    }
                }
                return response.body().read_to_end(fileStream->streambuf());
            })
            // Close the file stream.
            .then([=](size_t) {
                return fileStream->close();
            });

    // Plik wyjsciowy zawsze sie stworzy
    // Wait for all the outstanding I/O to complete and handle any exceptions
    try{
        requestTask.wait();

        if(returnedHttpCode!=200){
            return returnedHttpCode;
        }
        
    } catch (const std::exception &e){
        return -5;
        //printf("Error exception:%s\n", e.what());
    }
    return returnedHttpCode;
}

json::value RestServerConnector::returnUserDTOAsJson(std::string userInputIndex, std::string userInputCode){
        initUserDetails->setIndex(userInputIndex);
        initUserDetails->setUniqueCode(userInputCode);
        json::value json_par = json_par.parse(initUserDetails->toJson());
        return json_par;
}

UserDetailsDTO& RestServerConnector::getUserDetailsDTO(){
    return *initUserDetails;
}

const std::string RestServerConnector::getRegistrationFilePath(){
    return registrationFilePath;
}

void RestServerConnector::setUserDetailsDTO(UserDetailsDTO userInputDetails){
    *initUserDetails = userInputDetails;
}

std::string RestServerConnector::encryptAES256WithOpenSSL(std::string strToEncrypt){

    //DONT HARDCODE KEY! 
    openSSLCryptoUtil->setKey((unsigned char *)"mZq4t7w!z%C*F-J@NcRfUjXn2r5u8x/A");
    //DONT HARDCODE INITIALIZATION VECTOR!
    openSSLCryptoUtil->setIV((unsigned char *)"0000000000000000");
    unsigned char* charsToEncrypt = (unsigned char *)strToEncrypt.c_str();

    /*
     * Buffer for ciphertext. Ensure the buffer is long enough for the
     * ciphertext which may be longer than the plaintext, depending on the
     * algorithm and mode.
    */
    unsigned char ciphertext[openSSLCryptoUtil->getBufferLength()];

    /* Encrypt the plaintext */
    int ciphertext_l = openSSLCryptoUtil->encrypt(
    charsToEncrypt, 
    strlen((char *)charsToEncrypt),
    openSSLCryptoUtil->getKey(),
    openSSLCryptoUtil->getIV(),
    ciphertext);

    openSSLCryptoUtil->setCipherTextLength(ciphertext_l);
    openSSLCryptoUtil->setCipherText(ciphertext,ciphertext_l);

    std::cout << std::endl << ciphertext;
    std::string str_encode_base64 = openSSLCryptoUtil->base64_encode(ciphertext,ciphertext_l);

    std:: cout << "\nEncrypted text 64encoded: " << str_encode_base64;
    std:: cout << "\nEncrypted text 64decoded: " << openSSLCryptoUtil->base64_decode(str_encode_base64) << std::endl;

    return str_encode_base64;
}

std::string RestServerConnector::decryptAES256WithOpenSSL(std::string strToDecrypt){

    /* Buffer for the decrypted text */
    unsigned char decryptedtext[openSSLCryptoUtil->getBufferLength()];
    unsigned char encryptedtextTmp[openSSLCryptoUtil->getBufferLength()];
    strcpy((char *)encryptedtextTmp, openSSLCryptoUtil->getCipherTextAsString().c_str());

    /* Decrypt the ciphertext */
    int decryptedtext_l = openSSLCryptoUtil->decrypt(
    encryptedtextTmp, openSSLCryptoUtil->getCipherTextLength(),
    openSSLCryptoUtil->getKey(),
    openSSLCryptoUtil->getIV(),
    decryptedtext);

    openSSLCryptoUtil->setDecryptedTextLength(decryptedtext_l);
    openSSLCryptoUtil->setDecryptedText(decryptedtext,decryptedtext_l);

    /* Add a NULL terminator. We are expecting printable text */
    decryptedtext[decryptedtext_l] = '\0';

    /* Show the decrypted text */
    printf("\nDecrypted text is: ");
    printf("%s\n", decryptedtext);

    return (char *)decryptedtext;
}


