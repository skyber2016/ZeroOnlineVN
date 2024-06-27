#include "default_action.h"
int WINAPI DefaultAction::Send(const char* buf, int len, int flag)
{
	return this->CallSend(buf, len, flag);
}


int WINAPI DefaultAction::Recv(char* buf, int len, int flag)
{
	return this->CallRecv(buf, len, flag);
}