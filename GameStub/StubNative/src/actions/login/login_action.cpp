#include "login_action.h"
int WINAPI LoginAction::Send(const char* buf, int len, int flag)
{
	return this->CallSend(buf, len, flag);
}


int WINAPI LoginAction::Recv(char* buf, int len, int flag)
{
	return this->CallRecv(buf, len, flag);
}