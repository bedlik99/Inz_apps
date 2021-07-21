add_library(RESTConnector RestServerConnector.cpp)

set(CMAKE_CXX_STANDARD 17)
set(CMAKE_CXX_STANDARD_REQUIRED True)

add_subdirectory(IOConfig)
add_subdirectory(OPENSSL_AES_256)

list(APPEND IO_CONFIG_LIB IOConfig)
list(APPEND AES_256_LIB OpenSSLAesEncryptor)

target_link_libraries(RESTConnector PUBLIC 
${IO_CONFIG_LIB} 
${AES_256_LIB}
)

target_include_directories(RESTConnector
          INTERFACE ${CMAKE_CURRENT_SOURCE_DIR}
          )