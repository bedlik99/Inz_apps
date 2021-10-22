#include <string>
#include <cpprest/http_client.h>
#include "OPENSSL_AES_256/OpenSSLAesEncryptor.h"

using namespace web;

class RestServerConnector {

private:
    const std::string recordEventEndpoint = "/recordEvent"; // /api/recordEvent
    const std::string registerUserEndpoint = "/registerUser"; // /api/registerUser
    const std::string serverResponseFile = "/home/cerber/Documents/lab_supervision/identify_lab_data/server_responses";
    const std::string serverUrl = "http://localhost:8080"; // https://serverapieiti.azurewebsites.net
    OpenSSLAesEncryptor* openSSLCryptoUtil = nullptr;

    int executeRequest(json::value json_par, std::string filePath, std::string endpointName);
public:
    RestServerConnector();
    ~RestServerConnector();
    int sendData(std::string email,std::string uniqueCode, std::string registryContent,bool isRegistration);
};