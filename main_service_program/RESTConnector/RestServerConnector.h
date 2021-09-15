#include <string>
#include <cpprest/http_client.h>
#include "OPENSSL_AES_256/OpenSSLAesEncryptor.h"

using namespace web;

class RestServerConnector {

private:
    const std::string recordEventEndpoint = "/recordEvent";
    const std::string registerUserEndpoint = "/registerUser";
    const std::string serverResponseFile = "/etc/identify_lab_data/server_responses";
    const std::string serverUrl = "http://localhost:8080";
    OpenSSLAesEncryptor* openSSLCryptoUtil = nullptr;

    json::value returnEncryptedMessageDTO(std::string userInputIndex, std::string registryContent,bool isRegistration);
    int executeRequest(json::value json_par, std::string filePath, std::string endpointName);
public:
    RestServerConnector();
    ~RestServerConnector();
    int sendData(std::string inputIndex, std::string inputCode,bool isRegistration);
};