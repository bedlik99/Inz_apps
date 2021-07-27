#include <string>
#include <cpprest/http_client.h>
#include "OPENSSL_AES_256/OpenSSLAesEncryptor.h"

using namespace web;

class RestServerConnector {

private:
    const std::string registerEndpoint = "/registerUser";
    const std::string registrationFilePath = "/home/jan/Documents/inz_dyp/Projekty_C++/working_folder_inz/rgstr_trace/rqrm";
    const std::string serverUrl = "http://localhost:8080";
    OpenSSLAesEncryptor* openSSLCryptoUtil = nullptr;

    json::value returnEncryptedMessageDTO(std::string userIndex, std::string userCode);
    int executeRequest(json::value json_par, std::string filePath, std::string endpointName);
public:
    RestServerConnector();
    ~RestServerConnector();
    int sendData(std::string inputIndex, std::string inputCode);

    const std::string getRegistrationFilePath();


};