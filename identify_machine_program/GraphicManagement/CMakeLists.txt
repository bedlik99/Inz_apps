add_library(GraphicManager GraphicManager.cpp)

target_include_directories(GraphicManager
          INTERFACE ${CMAKE_CURRENT_SOURCE_DIR}
          )