add_library(RESTConnector RestServerConnector.cpp)

target_include_directories(RESTConnector
          INTERFACE ${CMAKE_CURRENT_SOURCE_DIR}
          )