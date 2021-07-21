#include <string>
#include <cpprest/http_client.h>
#include "OPENSSL_AES_256/OpenSSLAesEncryptor.h"

using namespace web;

class RestServerConnector {

private:
    const std::string recordEventEndpoint = "/recordEvent";
    const std::string registrationFilePath = "/home/jan/Documents/inz_dyp/Projekty_C++/working_folder_inz/rgstr_trace/rqrm";
    const std::string serverResponseFile = "/home/jan/Documents/inz_dyp/Projekty_C++/working_folder_inz/output_files/server_responses";
    const std::string serverUrl = "http://localhost:8080";
    OpenSSLAesEncryptor* openSSLCryptoUtil = new OpenSSLAesEncryptor();

    json::value returnEncryptedMessageDTO(std::string userIndex, std::string userCode);
    int executeRequest(json::value json_par, std::string filePath, std::string endpointName);
public:
    RestServerConnector();
    ~RestServerConnector();
    int sendData(std::string inputIndex, std::string inputCode);
    const std::string getRegistrationFilePath();
    OpenSSLAesEncryptor* getOpenSSLCryptoUtil();

};