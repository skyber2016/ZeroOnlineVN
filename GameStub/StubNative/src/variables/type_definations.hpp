#ifndef TYPE_DEFINATIONS_HPP
#define TYPE_DEFINATIONS_HPP

#include <winsock.h>

typedef int (WINAPI* send_t)(SOCKET, const char*, int, int);
typedef int (WINAPI* recv_t)(SOCKET, char*, int, int);
typedef int (WINAPI* connect_t)(SOCKET, const sockaddr*, int);

#endif // !TYPE_DEFINATIONS_H
