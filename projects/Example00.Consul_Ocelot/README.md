# Consul ע�������� Ocelot ����ʾ����Ŀ

## ʹ�÷�ʽ

1. ���� [consul.exe](https://www.consul.io/downloads.html)����ͨ���������� `consul agent -dev`
2. ����Ŀ������
3. �ֱ���� bin �ļ��У�ִ���������  
  `Example00.Consul_Ocelot.Server.exe --port="6666"`  
  `Example00.Consul_Ocelot.Server.exe --port="6667"`  
  `Example00.Consul_Ocelot.Server.exe --port="6668"`
4. ������Ŀ `Example00.Consul_Ocelot.Gateway`�����û�е��� consul ����ע��ҳ�棬��ô�ֶ���������д� <http://127.0.0.1:8500/>
5. ��ʱӦ�ÿ��Կ�����������Ľ�����鱻���ã��������ʧ����ô��� consul �����磬Ȼ��Ӧ�ÿ��Կ���ע����ȥ�ķ���
6. ͨ������ <http://127.0.0.1:6665/Test> ���ɿ���Ч��
