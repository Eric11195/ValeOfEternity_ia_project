using UnityEngine;

using System.Collections;

namespace voe{

public class CoroutineWithData<T> {
        public Coroutine coroutine { get; private set; }
        public T result;
        private IEnumerator target;

        public CoroutineWithData(MonoBehaviour owner, IEnumerator target) {
            this.target = target;
            this.coroutine = owner.StartCoroutine(Run());
        }

        private IEnumerator Run() {
            while(target.MoveNext()) {
                result = (T)target.Current;
                yield return result;
            }
        }
    }
}


//EXAMPLE OF USE
/*

private IEnumerator LoadSomeStuff( ) {
    WWW www = new WWW("http://someurl");
    yield return www;
    if (String.IsNullOrEmpty(www.error) {
        yield return "success";
    } else {
        yield return "fail";
    }
}

CoroutineWithData cd = new CoroutineWithData(this, LoadSomeStuff( ) );
yield return cd.coroutine;
Debug.Log("result is " + cd.result);  //  'success' or 'fail'
*/