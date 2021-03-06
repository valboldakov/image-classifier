#ifndef IMAGE_CLASSIFIER_LIB_IMAGE_CLASSIFIER_LIB_H
#define IMAGE_CLASSIFIER_LIB_IMAGE_CLASSIFIER_LIB_H

extern "C" {
/** @brief Init model from specified path.
 *
 * @param[in] path_to_model path to the model.
 * @return 0 in case of success.
 * @return -1 couldn't open load and init path_to_model.
 */
int32_t load_model(char *path_to_model);

/** @brief Evaluate loaded model.
 *
 * @param[in] data array of floats for the input layer
 * @param[in] size size of the input data array
 * @param[in] labels_amount amount of labels recognizable with model
 * @param[out] label_num number of the label with the highest probability
 * @return 0 in case of success
 * @return -1 in case of success torch internal error
 */
int32_t eval_model(const float *data, uint32_t size, uint32_t labels_amount, uint32_t *label_num);

/** @brief Write the last error to message variable.
 *
 * @param[out] message is an array to write message to
 * @param[in] max_message_size is maximum bytes available to write to
 * @return 0 in case of success
 */
int32_t get_last_error(char *message, uint32_t max_message_size);
}

#endif
