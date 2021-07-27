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

RestServerConnector::RestServerConnector(){
    openSSLCryptoUtil = new OpenSSLAesEncryptor();
}

RestServerConnector::~RestServerConnector(){
    if(openSSLCryptoUtil != nullptr){
        delete openSSLCryptoUtil;
        openSSLCryptoUtil = nullptr;
    }
}

int RestServerConnector::sendData(std::string inputIndex, std::string inputCode) {
    IOConfig ioConfig = IOConfig();
    if(!ioConfig.doesFileExist(registrationFilePath)){
        return executeRequest(returnEncryptedMessageDTO(inputIndex,inputCode), registrationFilePath, registerEndpoint);    
    }else if(ioConfig.isSecretFileEmpty(registrationFilePath)){
        return executeRequest(returnEncryptedMessageDTO(inputIndex,inputCode), registrationFilePath, registerEndpoint);
    }
    // this should not happen
    return -1;
}

int RestServerConnector::executeRequest(json::value json_par, std::string filePath, std::string endpointName){
    //static IOConfig ioConfig = IOConfig();
    static int returnedHttpCode;
    static std::string responseString;
    auto fileStream = std::make_shared<ostream>();

    pplx::task<void> requestTask =
        
        fstream::open_ostream(filePath)

            .then([=](ostream outFile) {
                *fileStream = outFile;

                // Create http_client to send the request.
                http_client client(serverUrl);

                uri_builder builder(endpointName);
                //builder.append_query("q", "facebook");
        
                return client.request(methods::POST, builder.to_string(),json_par);
            })

            // Handle response headers arriving.
            .then([=](http_response response) {
                returnedHttpCode = response.status_code();
                // responseString = response.extract_string().get();   

                // if(ioConfig.trim(responseString).length()!=0){
                //     //encrypt incoming data here
                //     responseString = openSSLCryptoUtil->encryptAES256WithOpenSSL(responseString);

                //     for(int i=0; i<responseString.length();i++){
                //         fileStream->streambuf().putc(responseString[i]);
                //     }
                // }
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

    } catch (const std::exception &e){
        return -5;
    }
    return returnedHttpCode;
}

json::value RestServerConnector::returnEncryptedMessageDTO(std::string userInputIndex, std::string userInputCode){
    std::string userDataAsJson = "{\"uniqueCode\":\""+userInputCode+"\",\"indexNr\":\""+userInputIndex+"\"}";
    std::string encryptedValue = openSSLCryptoUtil->encryptAES256WithOpenSSL(userDataAsJson);
    std::string encryptedMessageString = "{\"value\":\""+encryptedValue+"\"}";
    json::value json_encrypted_message = json_encrypted_message.parse(encryptedMessageString);

    return json_encrypted_message;
}

const std::string RestServerConnector::getRegistrationFilePath(){
    return registrationFilePath;
}