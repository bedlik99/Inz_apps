add_library(RESTConnector RestServerConnector.cpp)

add_subdirectory(IOConfig)
add_subdirectory(UserDetailsDTO)

list(APPEND IO_CONFIG_LIB IOConfig)
list(APPEND USER_DETAILS_LIB UserDetailsDTO)

target_link_libraries(RESTConnector PUBLIC ${IO_CONFIG_LIB} ${USER_DETAILS_LIB})

target_include_directories(RESTConnector
          INTERFACE ${CMAKE_CURRENT_SOURCE_DIR}
          )