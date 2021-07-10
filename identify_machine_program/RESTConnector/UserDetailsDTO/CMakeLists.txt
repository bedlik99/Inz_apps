add_library(UserDetailsDTO UserDetailsDTO.cpp)

target_include_directories(UserDetailsDTO
          INTERFACE ${CMAKE_CURRENT_SOURCE_DIR}
          )