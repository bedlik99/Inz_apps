#include <string>
#include <cpprest/http_client.h>

using namespace web;  

//Ten connector bedzie od obsluzenia wysylania logow na serwer - DataRecordDTO.cpp,DataRecordDTO.h
class RestServerConnector {

private:
    std::string dataRecordContent = "...";
    const std::string dataRecordEndpoint = "/registerUser";
    const std::string recordsFilePath = "/home/jan/Documents/inz_dyp/Projekty_C++/working_folder_inz/outputFiles/rcrds.txt";
    static DataRecordDTO dataRecord;

public:
    RestServerConnector();
    void startRequest();
    json::value returnRecordDataAsJson();
    void processRequest(json::value json_par, std::string filePath, std::string endpointName);

};