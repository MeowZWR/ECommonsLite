using ImGuiNET;
using System;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using Action = System.Action;

namespace ECommonsLite.ImGuiMethods;
#nullable disable

public static unsafe partial class ImGuiEx
{
    public static void EzTabBar(string id, params (string name, Action function, Vector4? color, bool child)[] tabs) => EzTabBar(id, null, tabs);
    public static void EzTabBar(string id, string KoFiTransparent, params (string name, Action function, Vector4? color, bool child)[] tabs) => EzTabBar(id, KoFiTransparent, null, tabs);
    public static void EzTabBar(string id, string KoFiTransparent, string openTabName, params (string name, Action function, Vector4? color, bool child)[] tabs) => EzTabBar(id, KoFiTransparent, openTabName, ImGuiTabBarFlags.None, tabs);
    public static void EzTabBar(string id, string KoFiTransparent, string openTabName, ImGuiTabBarFlags flags, params (string name, Action function, Vector4? color, bool child)[] tabs)
    {
        ImGui.BeginTabBar(id, flags);
        foreach (var x in tabs)
        {
            if (x.name == null) continue;
            if (x.color != null)
            {
                ImGui.PushStyleColor(ImGuiCol.Text, x.color.Value);
            }
            if (ImGuiEx.BeginTabItem(x.name, openTabName == x.name ? ImGuiTabItemFlags.SetSelected : ImGuiTabItemFlags.None))
            {
                if (x.color != null)
                {
                    ImGui.PopStyleColor();
                }
                if (x.child) ImGui.BeginChild(x.name + "child");
                x.function();
                if (x.child) ImGui.EndChild();
                ImGui.EndTabItem();
            }
            else
            {
                if (x.color != null)
                {
                    ImGui.PopStyleColor();
                }
            }
        }
        //if (KoFiTransparent != null) PatreonBanner.RightTransparentTab();
        ImGui.EndTabBar();
    }

    public static unsafe bool BeginTabItem(string label, ImGuiTabItemFlags flags)
    {
        var num = 0;
        byte* ptr;
        if (label != null)
        {
            num = Encoding.UTF8.GetByteCount(label);
            ptr = Allocate(num + 1);
            var utf = GetUtf8(label, ptr, num);
            ptr[utf] = 0;
        }
        else
        {
            ptr = null;
        }

        byte* p_open2 = null;
        var num2 = ImGuiNative.igBeginTabItem(ptr, p_open2, flags);
        if (num > 2048)
        {
            Free(ptr);
        }
        return num2 != 0;
    }
    internal static unsafe byte* Allocate(int byteCount)
    {
        return (byte*)(void*)Marshal.AllocHGlobal(byteCount);
    }

    internal static unsafe void Free(byte* ptr)
    {
        Marshal.FreeHGlobal((IntPtr)ptr);
    }

    internal static unsafe int GetUtf8(string s, byte* utf8Bytes, int utf8ByteCount)
    {
        fixed (char* chars = s)
        {
            return Encoding.UTF8.GetBytes(chars, s.Length, utf8Bytes, utf8ByteCount);
        }
    }
}
