#define CATCH_CONFIG_MAIN

#include <catch2/catch.hpp>
#include "../src/image_classifier_lib.h"


TEST_CASE("Loading model", "[load_model]") {
    int32_t res = load_model("path_to_model");
    REQUIRE(res == 0);
}

float test_image_data[784]{

};


TEST_CASE("Evaluating model", "[eval_model]") {
    uint32_t label_num = 0;
    int32_t res = eval_model(test_image_data, 784, &label_num);
    REQUIRE(res == 0);
    REQUIRE(label_num <= 9);
}
