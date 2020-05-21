#include <cstdint>
#include <torch/script.h>
#include "image_classifier_lib.h"

thread_local torch::jit::script::Module module;

thread_local std::string last_error;

int32_t load_model(char *path_to_model) {
    try {
        module = torch::jit::load(path_to_model);
    }
    catch (const c10::Error &error) {
        last_error = error.msg();
        return -1;
    }
    return 0;
}

int32_t eval_model(const float *data, uint32_t size, uint32_t labels_amount, uint32_t *label_num) {
    std::vector<float> raw_input_data(size, 0);
    memcpy(raw_input_data.data(), data, size * sizeof(float));
    float *out_ptr;
    try {
        at::Tensor input_data = torch::from_blob(raw_input_data.data(), {1, size});
        std::vector<torch::jit::IValue> input{input_data};
        at::Tensor output = torch::exp(module.forward(input).toTensor());
        out_ptr = output.data_ptr<float>();
    } catch (const c10::Error &error) {
        last_error = error.msg();
        return -1;
    }
    *label_num = 0;
    for (uint32_t i = 0; i < labels_amount; ++i) {
        if (out_ptr[i] > out_ptr[*label_num]) {
            *label_num = i;
        }
    }
    return 0;
}

int32_t get_last_error(char *message, uint32_t max_message_size) {
    uint32_t copy_len = std::min(static_cast<uint32_t>(last_error.length()), max_message_size - 1);
    memcpy(message, last_error.data(), copy_len);
    message[copy_len] = 0;
    return 0;
}
