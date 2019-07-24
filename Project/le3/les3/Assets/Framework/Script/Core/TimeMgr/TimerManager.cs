using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public interface ITimer
    {
        //开始计时器
        void Start();

        void Stop();

        void UpdateTimer();
    }

    public class Timer : ITimer
    {
        protected float mDuration = 1.0f;
        protected float mStartDuration = 1.0f;
        protected bool bStart = false;
        protected object mCallBackPara = null;

        //-1代表一直循环
        protected int mLoop = 1;

        public delegate void Callback(object o);

        Callback mCallBack;

        public Timer(Callback callBack, float duration = 1.0f, int loop = 1, object para = null)
        {
            mStartDuration = mDuration = duration;
            mCallBack = callBack;
            mLoop = loop;
            mCallBackPara = para;
        }
        public void Reset(float duration, int loop, Callback callBack, object para = null)
        {
            mDuration = duration;
            mCallBack = callBack;
            mLoop = loop;
            mCallBackPara = para;
        }

        public void Start()
        {
            TimerManager.Instance.AddTimer(this);
            bStart = true;
        }

        public void Stop()
        {
            TimerManager.Instance.RemoveTimer(this);

            bStart = false;
        }

        public void UpdateTimer()
        {
            if (bStart)
            {
                mDuration -= Time.deltaTime;
                if (mDuration < 0 && (mLoop > 0 || mLoop == -1))
                {
                    if (mLoop != -1)
                        mLoop -= 1;
                    mDuration = mStartDuration;
                    mCallBack(mCallBackPara);
                    bStart = (mLoop == 0 ? false : true);
                    if (mLoop == 0)
                    {
                        Stop();
                    }
                }
            }
        }
    }

    public class FrameTimer : ITimer
    {
        protected int mFrameCount = 1;
        protected int mStartFramecount = 1;

        private bool bStart = false;
        protected object mCallBackPara = null;


        //-1代表一直循环
        protected int mLoop = 1;

        public delegate void Callback(object para);

        Callback mCallBack;

        public FrameTimer(Callback callBack, int frameCount = 1, int loop = 1, object para = null)
        {
            mStartFramecount = mFrameCount = frameCount;
            mCallBack = callBack;
            mLoop = loop;
            mCallBackPara = para;

        }
        public void Reset(int frameCount, int loop, Callback callBack, object para = null)
        {
            mFrameCount = frameCount;
            mCallBack = callBack;
            mLoop = loop;
            mCallBackPara = para;
        }

        public void Start()
        {
            TimerManager.Instance.AddTimer(this);
            bStart = true;
        }

        public void Stop()
        {
            TimerManager.Instance.RemoveTimer(this);

            bStart = false;
        }

        public void UpdateTimer()
        {
            if (bStart)
            {
                mFrameCount -= 1;
                if (mFrameCount <= 0 && (mLoop > 0 || mLoop == -1))
                {
                    if (mLoop != -1)
                        mLoop -= 1;
                    mCallBack(mCallBackPara);
                    mFrameCount = mStartFramecount;
                    if (mLoop == 0)
                    {
                        Stop();
                    }

                }
            }
        }
    }


    public class TimerManager : BaseComponentTemplate<TimerManager>
    {
        protected List<ITimer> mTimerList = new List<ITimer>();

        public void Reset()
        {
            mTimerList.Clear();
        }

        public void LateUpdate()
        {
            for (int i = 0; i < mTimerList.Count; ++i)
            {
                mTimerList[i].UpdateTimer();
            }
        }

        public void AddTimer(ITimer timer)
        {
            mTimerList.Add(timer);
        }

        public bool RemoveTimer(ITimer timer)
        {
            return mTimerList.Remove(timer);
        }
    }

}
