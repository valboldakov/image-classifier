#include <cstdint>
#include <torch/script.h>
#include "image_classifier_lib.h"

torch::jit::script::Module module;

int32_t load_model(char *path_to_model) {
    try {
        module = torch::jit::load(path_to_model);
    }
    catch (const c10::Error &error) {
        return -1;
    }
    return 0;
}

int32_t eval_model(const float *data, uint32_t size, uint32_t *label_num) {
    return 0;
}
