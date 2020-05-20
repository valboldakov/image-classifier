cmake_minimum_required(VERSION 3.13)
project(image_classifier_lib)

set(CMAKE_CXX_STANDARD 14)
find_package(Torch REQUIRED)
find_package(Catch2 REQUIRED)

set(IMAGE_CLASSIFIER_LIB_SRC "src/image_classifier_lib.h" "src/image_classifier_lib.cpp")

add_executable(image_classifier_lib_test tests/image_classifier_lib_test.cpp ${IMAGE_CLASSIFIER_LIB_SRC})
target_link_libraries(image_classifier_lib_test Catch2::Catch2 ${TORCH_LIBRARIES})
