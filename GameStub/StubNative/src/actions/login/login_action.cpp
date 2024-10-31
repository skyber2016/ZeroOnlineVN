#include "login_action.h"
int WINAPI LoginAction::Send(SOCKET s, const char* buf, int len, int flag)
{
	return this->CallSend(s, buf, len, flag);
}


int WINAPI LoginAction::Recv(SOCKET s, char* buf, int len, int flag)
{
	return this->CallRecv(s, buf, len, flag);
}