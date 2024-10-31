#include "default_action.h"
int WINAPI DefaultAction::Send(SOCKET s, const char* buf, int len, int flag)
{
	return this->CallSend(s, buf, len, flag);
}


int WINAPI DefaultAction::Recv(SOCKET s, char* buf, int len, int flag)
{
	return this->CallRecv(s, buf, len, flag);
}