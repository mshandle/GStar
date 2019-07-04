//-------------------------------------------------------------------------------------
//	TemplateMethodStructure.cs
//-------------------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class TemplateMethodStructure : MonoBehaviour
{
	void Start ( )
	{
        Application framework = new Application();
        framework.InitStep1();
        ApplicationXXX app = new ApplicationXXX();

        if (app.InitStep2())
        {
            framework.InitStep3();
        }
        else
        {
            framework.InitStep4();
        }
    }
}

/// <summary>
/// The 'AbstractClass' abstract class
/// </summary>
class Application
{
    public  void InitStep1()
    {
        //..
    }
    public  void InitStep3()
    {
        //..
    }

    public void InitStep4()
    {
        //..
    }
}

/// <summary>
/// A 'ConcreteClass' class
/// </summary>
class ApplicationXXX : Application
{
   public bool InitStep2()
    {
        //...
        return true;
    }
}



namespace Test
{

    abstract public class Application
    {
        public void Init()
        {
            InitStep1();
            if (InitStep2())
            {
                InitStep3();
            }
            else
            {
                InitStep4();
            }
        }

        private void InitStep1()
        {
            //..
        }
        private void InitStep3()
        {
            //..
        }

        private void InitStep4()
        {
            //..
        }

        protected abstract bool InitStep2();
    }

    public class ApllicationBB : Application
    {
        protected override bool InitStep2()
        {
            return true;
        }
    }


    public class TemplateMethodStructure : MonoBehaviour
    {
        void Start()
        {
            Application framework = new ApllicationBB();
            framework.Init();
        }
    }
}

