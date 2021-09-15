#!/bin/bash
cd identify_machine_program
cmake --build .
cd ../main_service_program
cmake --build .
cd ../module_service_program
cmake --build .