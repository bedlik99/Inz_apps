#include <cpprest/http_client.h>
#include <cpprest/filestream.h>
#include <string>
#include "IOConfig/IOConfig.h"
#include "RestServerConnector.h"

using namespace utility;              // Common utilities like string conversions
using namespace web;                  // Common features like URIs.
using namespace web::http;            // Common HTTP functionality
using namespace web::http::client;    // HTTP client features
using namespace concurrency::streams; // Asynchronous streams

RestServerConnector::RestServerConnector(){}

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
    static int returnedHttpCode;
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
                
                printf("Received response status code:%u\n", response.status_code());
                returnedHttpCode = response.status_code();

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
        initUserDetails.setIndex(userInputIndex);
        initUserDetails.setUniqueCode(userInputCode);
        json::value json_par = json_par.parse(initUserDetails.toJson());
        return json_par;
}

UserDetailsDTO& RestServerConnector::getUserDetailsDTO(){
    return initUserDetails;
}

const std::string RestServerConnector::getRegistrationFilePath(){
    return registrationFilePath;
}

void RestServerConnector::setUserDetailsDTO(UserDetailsDTO userInputDetails){
    initUserDetails = userInputDetails;
}