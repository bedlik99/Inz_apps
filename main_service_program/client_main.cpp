#include <cpprest/http_client.h>
#include <cpprest/filestream.h>
#include <sys/ptrace.h>
#include <string>
#include "FinalStudentDetails.h"
#include "InitStudentDetails.h"
#include "IOConfig.h"

using namespace utility;              // Common utilities like string conversions
using namespace web;                  // Common features like URIs.
using namespace web::http;            // Common HTTP functionality
using namespace web::http::client;    // HTTP client features
using namespace concurrency::streams; // Asynchronous streams

void doTask(json::value,std::string,std::string);
json::value returnInitStudentDetailsAsJson();
json::value returnFinalStudentDetailsAsJson();

const std::string student_index_num = "300500";
const std::string student_unique_code = "gK4L0";
const std::string initialEndpoint = "/sendInit";
const std::string secretFilePath = "/home/jan/Documents/inz_dyp/Projekty_C++/working_folder_inz/outputFiles/secretFile.txt";
const std::string finalizeEndpoint = "/sendFinal";
const std::string resultInfoFilePath = "/home/jan/Documents/inz_dyp/Projekty_C++/working_folder_inz/outputFiles/results.txt";

// DO ZROBIENIA:
// ZMIANA NAZW PLIKOW OUTPUTOWYCH NA dataFile0,dataFile1,dataFile2... (nazwa tworzona dynamicznie)
// Zmienic zawartosc pliku client_main.cpp na klase RestServerConnector.cpp i dodac odpowiedni plik RestServerConnector.h
// w nowym folderze. Zatem, zmodyfikować też CMakeLists.txt

static IOConfig ioConfig;
static InitStudentDetails initStudentDetails;
static int returnedHttpCode;
static bool wasSecretFileInitProperly=false;

int main(void) {
    if (ptrace(PTRACE_TRACEME, 0, 0, 0) < 0) {
        printf("A debugger is attached, but not for long!\n");
        kill(getpid(), SIGTERM);
        exit(0);
    }

    // if (option == 1 && (!ioConfig.doesFileExist(secretFilePath) || ioConfig.isSecretFileEmpty(secretFilePath,wasSecretFileInitProperly))){
    //     doTask(returnInitStudentDetailsAsJson(), secretFilePath, initialEndpoint);
    // }
    if(ioConfig.doesFileExist(secretFilePath) && !ioConfig.isSecretFileEmpty(secretFilePath,wasSecretFileInitProperly)){
        doTask(returnFinalStudentDetailsAsJson(), resultInfoFilePath, finalizeEndpoint);

    }else if(wasSecretFileInitProperly){
        std::cout<<"Machine was already identified.\n";

    }else{
        std::cout<<"Machine was not identified properly. Student should identify machine again.\n";
    }
    return 1;
}

void doTask(json::value json_par, std::string fileName, std::string endpointName){

    auto fileStream = std::make_shared<ostream>();

    pplx::task<void> requestTask =
        fstream::open_ostream(fileName)

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

json::value returnInitStudentDetailsAsJson(){
        initStudentDetails.setIndexNum(student_index_num);
        initStudentDetails.setDateTime("2021-06-10 16:28");
        json::value json_par = json_par.parse(initStudentDetails.toJson());
        
        return json_par;
}

json::value returnFinalStudentDetailsAsJson(){
    std::ifstream t(secretFilePath);
    std::string secretHash((std::istreambuf_iterator<char>(t)),std::istreambuf_iterator<char>());

    FinalStudentDetails finalStudentDetails(ioConfig.trim(secretHash),student_index_num,
                                            student_unique_code, 5.0, "2021-06-12 10:20");
    json::value json_par = json_par.parse(finalStudentDetails.toJson());
    return json_par;
}

//debugowanie skryptu bashowego: bash -x ./{nazwaPliku}