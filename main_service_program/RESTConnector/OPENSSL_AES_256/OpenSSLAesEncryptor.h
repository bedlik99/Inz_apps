#include <string>
#include <vector>

class OpenSSLAesEncryptor {
private:
    const std::string base64_chars = 
    "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
    "abcdefghijklmnopqrstuvwxyz"
    "0123456789+/";
    const std::string writable_chars = "P_3Auw|g!4EHS1.#W0<+oR?OIZ'9k*6=hCUGtTbQN(f;7/%lr8>LzD2$sy5p@Mq,acBdveKV)nm~ij`Y:&JXF^x-";
    const static int bufferLength = 8192;
    const std::string secretPath = "/home/cerber/Documents/lab_supervision/identify_lab_data/keys";

    std::string base64_encode(unsigned char* bytes_to_encode, int in_len);
    std::string base64_decode(std::string & encoded_string);
    void handleErrors(void);
    int decrypt(unsigned char *ciphertext, int ciphertext_len, unsigned char *key,unsigned char *iv, unsigned char *plaintext);
    int encrypt(unsigned char *plaintext, int plaintext_len, unsigned char *key, unsigned char *iv, unsigned char *ciphertext);
    void readSecrects(std::string& key, std::string& iv);
    bool fillStringWithChars(std::string& str);
    void removeCharsFromString(std::string& str);
    std::string convert_ASCII_To16System(int charASCIIValue);
    long findMaxRangeOfStringLength(long strLength, long lowRange, long highRange);
    int convert_ASCII_To10System(char charASCIIValue);
public:
    OpenSSLAesEncryptor();
    std::string encryptAES256WithOpenSSL(std::string strToEncrypt);
    std::string decryptAES256WithOpenSSL(std::string encoded64StrToDecrypt);
};