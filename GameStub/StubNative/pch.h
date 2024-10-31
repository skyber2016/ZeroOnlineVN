// pch.h: This is a precompiled header file.
// Files listed below are compiled only once, improving build performance for future builds.
// This also affects IntelliSense performance, including code completion and many code browsing features.
// However, files listed here are ALL re-compiled if any one of them is updated between builds.
// Do not add files here that you will be updating frequently as this negates the performance advantage.
#pragma comment(lib, "detours.lib")
#pragma comment(lib, "d3d8.lib")
#pragma comment(lib, "psapi.lib")

#ifndef PCH_H
#define PCH_H

// add headers that you want to pre-compile here
#include "framework.h"
#include "src/sockets/socket.h"
#include <iostream>
#include "src/variables/global_variables.hpp"
#include "src/hooks/hook_managers.h"
#include "src/helpers/win_helper.hpp"
#include <detours.h>
#endif //PCH_H
