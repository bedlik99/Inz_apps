#include <cpprest/http_client.h>
#include <cpprest/filestream.h>
#include <sys/ptrace.h>
#include <string>
#include "IOConfig.h"
#include "RestServerConnector.h"

using namespace utility;              // Common utilities like string conversions
using namespace web;                  // Common features like URIs.
using namespace web::http;            // Common HTTP functionality
using namespace web::http::client;    // HTTP client features
using namespace concurrency::streams; // Asynchronous streams

RestServerConnector::RestServerConnector(){}

void RestServerConnector::startRequest() {
    bool wasSecretFileInitProperly=false;
    IOConfig ioConfig = IOConfig();

    // if (option == 1 && (!ioConfig.doesFileExist(secretFilePath) || ioConfig.isSecretFileEmpty(secretFilePath,wasSecretFileInitProperly))){
    //     doTask(returnInitStudentDetailsAsJson(), secretFilePath, initialEndpoint);
    // }
    if(!ioConfig.doesFileExist(registrationFilePath) || !ioConfig.isSecretFileEmpty(registrationFilePath,wasSecretFileInitProperly)){
        processRequest(returnUserDTOAsJson(), registrationFilePath, registerEndpoint);

    }else if(wasSecretFileInitProperly){
        std::cout<<"Machine was already identified.\n";

    }else{
        std::cout<<"Machine was not identified properly. Student should identify machine again.\n";
    }

}

void RestServerConnector::processRequest(json::value json_par, std::string filePath, std::string endpointName){
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

        if(returnedHttpCode==406){
            std::cout<< "Verify your credentials" << std::endl;
            // std::remove(secretFilePath.c_str());
        }
        
    }
    catch (const std::exception &e){
        printf("Error exception:%s\n", e.what());
    }

}

json::value RestServerConnector::returnUserDTOAsJson(){
        dataRecord.setIndexNum(dataRecordContent);
        dataRecord.setDateTime("2021-06-10 16:28");
        json::value json_par = json_par.parse(dataRecord.toJson());
        
        return json_par;
}