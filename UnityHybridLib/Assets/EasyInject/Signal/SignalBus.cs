using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;


public class SignalBus 
{
    class CallerInfo
    {
        public object rawCallback;
        public Action<object> callback;
    }


    private Dictionary<string, List<CallerInfo>> m_caller = new Dictionary<string, List<CallerInfo>>();
    private Dictionary<string, Type> m_command = new Dictionary<string, Type>();


    /// <summary>
    /// subscrib signal
    /// </summary>
    /// <typeparam name="TSignal"></typeparam>
    /// <param name="callback"></param>
    public void Subscribe<TSignal>(Action<TSignal> callback)
    {
        string typeName = typeof(TSignal).Name;

        if(m_caller.ContainsKey(typeName) == false)
        {
            m_caller.Add(typeName, new List<CallerInfo>());
        }

        // check if already subscribed
        bool alreadyExist = false;
        List<CallerInfo> callers = m_caller[typeName];
        foreach(CallerInfo caller in callers)
        {
            if((Action<TSignal>)caller.rawCallback == callback)
            {
                alreadyExist = true;
                break;
            }
        }

        Assert.IsFalse(alreadyExist, $"Subscribe {typeof(TSignal).Name} already exist.");

        CallerInfo ci = new CallerInfo();
        ci.rawCallback = callback;
        ci.callback = (obj) => callback.Invoke((TSignal)obj);

        m_caller[typeName].Add(ci);
    }

    /// <summary>
    /// unsubscrib signal
    /// </summary>
    /// <typeparam name="TSignal"></typeparam>
    /// <param name="callback"></param>
    /// <returns></returns>
    public bool Unsubscribe<TSignal>(Action<TSignal> callback)
    {
        string typeName = typeof(TSignal).Name;

        if(m_caller.ContainsKey(typeName))
        {
            List<CallerInfo> cis = m_caller[typeName];
            CallerInfo removedCi = null;

            foreach(CallerInfo ci in cis)
            {
                if((Action<TSignal>)ci.rawCallback == callback)
                {
                    removedCi = ci;
                    break;
                }
            }

            if (removedCi == null)
            {
                return false;
            }

            cis.Remove(removedCi);

            return true;
        }

        return false;
    }

    public bool RegisitCommand<TSignal>(Type commandType) 
    {
        string typeName = typeof(TSignal).Name;

        if(m_command.ContainsKey(typeName))
        {
            return false;
        }

        m_command.Add(typeName, commandType);

        return true;
    }


    public void Fire(object signal)
    {
        string typeName = signal.GetType().Name;

        if(m_caller.ContainsKey(typeName))
        {
            foreach(var caller in m_caller[typeName])
            {
                caller.callback.Invoke(signal);
            }
        }

        if(m_command.ContainsKey(typeName))
        {
            Type commandType = m_command[typeName];

            BaseCmd command = Activator.CreateInstance(commandType) as BaseCmd;

            command.SIGNAL = signal;
            command.Exec();
        }
    }

}
