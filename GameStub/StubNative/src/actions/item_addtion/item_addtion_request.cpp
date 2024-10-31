#include "item_addtion_request.h"
int WINAPI ItemAddtionRequest::Send(SOCKET s, const char* buf, int len, int flag)
{
	return CallSend(s, buf, len, flag);
}
int WINAPI ItemAddtionRequest::Recv(SOCKET s, char* buf, int len, int flag)
{
	ItemAddtionPacket* packet = reinterpret_cast<ItemAddtionPacket*>((int*)buf);

	std::cout << "item_addtion: First=" << packet->FirstItemId << " Second=" << packet->SecondItemId << std::endl;

	if (packet->FirstItemId != 0 && packet->SecondItemId != 0)
	{
		return 0;
	}
	return CallRecv(s, buf, len, flag);
}