FROM tensorflow/tensorflow:2.4.0-gpu-jupyter

RUN pip install gym==0.20.0 scipy==1.5.4 gym-unity==0.28.0
RUN mkdir /tf/logs

ENTRYPOINT tensorboard --logdir /tf/logs --port 8889 --host 0.0.0.0 & jupyter notebook --allow-root --port 8888 --ip 0.0.0.0
#ENTRYPOINT /bin/sh -c bash