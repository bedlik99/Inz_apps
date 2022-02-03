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
    const static int xor_a = 41, xor_b = 97, xor_c = 149;
    int k0=6, k1=172, k2=197, k3=90, k4=250, k5=192, k6=163, k7=48, k8=227, k9=244, k10=245, k11=141, k12=208, k13=25, k14=49, k15=124, k16=158,
        k17=21, k18=155, k19=53, k20=25, k21=73, k22=82, k23=208, k24=93, k25=190, k26=38, k27=246, k28=109, k29=221, k30=27, k31=251;
    int iv0=134, iv1=67, iv2=83, iv3=219, iv4=196, iv5=137, iv6=30, iv7=136, iv8=182, iv9=209, iv10=12, iv11=180, iv12=6, iv13=14, iv14=108, iv15=132;

    std::string base64_encode(unsigned char* bytes_to_encode, int in_len);
    std::string base64_decode(std::string & encoded_string);
    void handleErrors(void);
    int decrypt(unsigned char *ciphertext, int ciphertext_len, unsigned char *key,unsigned char *iv, unsigned char *plaintext);
    int encrypt(unsigned char *plaintext, int plaintext_len, unsigned char *key, unsigned char *iv, unsigned char *ciphertext);
    void createSecrets(std::string& key, std::string& iv);
    bool fillStringWithChars(std::string& str);
    void removeCharsFromString(std::string& str);
    std::string convert_ASCII_To16System(int charASCIIValue);
    long findMaxRangeOfStringLength(long strLength, long lowRange, long highRange);
    int convert_ASCII_To10System(char charASCIIValue);
    void reverseXOR(int &sign);
public:
    OpenSSLAesEncryptor();
    std::string encryptAES256WithOpenSSL(std::string strToEncrypt);
    std::string decryptAES256WithOpenSSL(std::string encoded64StrToDecrypt);
};