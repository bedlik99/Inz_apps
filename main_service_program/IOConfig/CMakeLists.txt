add_library(IOConfig IOConfig.cpp)

target_include_directories(IOConfig
          INTERFACE ${CMAKE_CURRENT_SOURCE_DIR}
          )