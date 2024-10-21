#ifndef BASE_ACTION_H
#define BASE_ACTION_H

#include <winsock.h>
#include "../datas/data_type.hpp"
#include "../variables/global_variables.hpp"
#include "../variables/type_definations.hpp"


class ActionFactory
{
protected:
	int WINAPI CallSend(SOCKET s, const char* buf, int len, int flag);
	int WINAPI CallRecv(SOCKET s, char* buf, int len, int flag);
public:

	virtual int WINAPI Send(SOCKET s, const char* buf, int len, int flag);
	virtual int WINAPI Recv(SOCKET s, char* buf, int len, int flag);

	ActionFactory* GetAction(const char* buf, int len, int flag);
};



#endif // !BASE_ACTION_H



