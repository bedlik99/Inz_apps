add_library(OpenSSLAesEncryptor OpenSSLAesEncryptor.cpp)


target_include_directories(OpenSSLAesEncryptor 
          INTERFACE ${CMAKE_CURRENT_SOURCE_DIR}
          )