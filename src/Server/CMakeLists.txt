cmake_minimum_required(VERSION 3.15)
project(server)

set(CMAKE_CXX_STANDARD 17)

include_directories(../../include)

set(COMMON_SOURCES ../common/Management.cpp ../../include/Management.h
        ../common/Bank.cpp ../../include/Bank.h
        )


set(SERVER_SOURCES main.cpp)

add_executable(server ${COMMON_SOURCES} ${SERVER_SOURCES})