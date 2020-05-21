FROM ubuntu:eoan as image-classifier-lib

RUN apt-get update && apt-get install -y git cmake g++ wget unzip
RUN git clone https://github.com/catchorg/Catch2.git /opt/catch2
RUN cd /opt/catch2/ && git checkout tags/v2.12.1 \ 
                    && cmake -Bbuild -H. -DBUILD_TESTING=OFF \ 
                    && cmake --build build/ --target install

RUN cd /opt && \
    wget https://download.pytorch.org/libtorch/nightly/cpu/libtorch-shared-with-deps-latest.zip && \
    unzip libtorch-shared-with-deps-latest.zip

WORKDIR /app
COPY image-classifier-lib image-classifier-lib
RUN cd image-classifier-lib/ && mkdir build && cd build && \
    cmake -DCMAKE_PREFIX_PATH=/opt/libtorch -D_GLIBCXX_USE_CXX11_ABI=0 -DCMAKE_BUILD_TYPE=Release .. && \
    make
COPY research/traced_model.pt ./research/traced_model.pt
RUN ./image-classifier-lib/build/image_classifier_lib_test
