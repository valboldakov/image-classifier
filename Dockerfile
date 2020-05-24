FROM ubuntu:bionic as image-classifier-lib

RUN apt-get update && apt-get install -y git cmake g++ wget unzip
RUN git clone https://github.com/catchorg/Catch2.git /opt/catch2
RUN cd /opt/catch2/ && git checkout tags/v2.12.1 \ 
                    && cmake -Bbuild -H. -DBUILD_TESTING=OFF \ 
                    && cmake --build build/ --target install

RUN cd /opt && \
    wget https://download.pytorch.org/libtorch/cpu/libtorch-cxx11-abi-shared-with-deps-1.5.0%2Bcpu.zip && \
    unzip libtorch-cxx11-abi-shared-with-deps-1.5.0+cpu.zip 

WORKDIR /app
COPY image-classifier-lib image-classifier-lib
RUN cd image-classifier-lib/ && mkdir build && cd build && \
    cmake -DCMAKE_PREFIX_PATH=/opt/libtorch -D_GLIBCXX_USE_CXX11_ABI=1 -DCMAKE_BUILD_TYPE=Release .. && \
    make
COPY research/traced_model.pt ./research/traced_model.pt
RUN ./image-classifier-lib/build/image_classifier_lib_test


FROM mcr.microsoft.com/dotnet/core/sdk:3.1-bionic AS image-classifier-service-build

RUN apt-get update && apt-get install -y libgdiplus

WORKDIR /app

COPY --from=image-classifier-lib /opt/libtorch /opt/libtorch
COPY --from=image-classifier-lib /app/image-classifier-lib/build/libimageclassifier.so /usr/lib
COPY service/ ./

RUN dotnet publish -o ./ImageClassifierService.Core
RUN mv ./Test/TestData Test/bin/Debug/netcoreapp3.1 && dotnet test


FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-bionic AS  image-classifier-service

WORKDIR app

RUN apt-get update && apt-get install -y libgdiplus

COPY --from=image-classifier-lib /opt/libtorch /opt/libtorch
COPY --from=image-classifier-lib /app/image-classifier-lib/build/libimageclassifier.so /usr/lib

COPY research/traced_model.pt research/traced_model.pt
COPY --from=image-classifier-service-build /app/ImageClassifierService.Core .
ENTRYPOINT ["./Core"]
