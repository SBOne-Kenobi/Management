cmake_minimum_required(VERSION 3.15)
project(Management)

add_subdirectory(src/Client)
add_subdirectory(src/Server)

add_custom_target(Management)
add_dependencies(Management client server)
