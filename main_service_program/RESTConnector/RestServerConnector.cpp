#include <cpprest/http_client.h>
#include <cpprest/filestream.h>
#include <string>
#include <vector>
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

OpenSSLAesEncryptor& RestServerConnector::getOpenSSLCryptoUtil(){
    return *openSSLCryptoUtil;
}

int RestServerConnector::sendData(std::string email,std::string uniqueCode, std::string registryContent,bool isRegistration) {
    std::string selectedEndpoint = (isRegistration ? registerUserEndpoint : recordEventEndpoint); 
    std::string userDataAsJson;
    if(isRegistration){
        userDataAsJson = "{\"uniqueCode\":\""+uniqueCode+"\",\"email\":\""+email+"\"}";
    }else{
        userDataAsJson = "{\"uniqueCode\":\""+uniqueCode+"\",\"registryContent\":\""+registryContent+"\"}";
    }
    std::string encryptedValue = openSSLCryptoUtil->encryptAES256WithOpenSSL(userDataAsJson);
    std::string encryptedMessageString = "{\"value\":\""+encryptedValue+"\"}";
    json::value json_encrypted_message = json_encrypted_message.parse(encryptedMessageString);
    
    return executeRequest(json_encrypted_message,serverResponseFile, selectedEndpoint);
}

int RestServerConnector::executeRequest(json::value json_par, std::string filePath, std::string endpointName){
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
        
                return client.request(methods::POST, builder.to_string(),json_par);
            })

            // Handle response headers arriving.
            .then([=](http_response response) {
                returnedHttpCode = response.status_code();
                // responseString = response.extract_string().get();   
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
        return 500;
    }
    return returnedHttpCode;
}