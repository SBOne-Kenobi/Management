cmake_minimum_required(VERSION 3.15)
project(client)

set(CMAKE_CXX_STANDARD 17)

include_directories(../../include)

set(COMMON_SOURCES ../common/Management.cpp)
set(CLIENT_SOURCES main.cpp)

add_executable(client ${COMMON_SOURCES} ${CLIENT_SOURCES})