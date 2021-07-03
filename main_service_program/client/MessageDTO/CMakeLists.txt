add_library(MessageDTO FinalStudentDetails.cpp InitStudentDetails.cpp)

target_include_directories(MessageDTO
          INTERFACE ${CMAKE_CURRENT_SOURCE_DIR}
          )