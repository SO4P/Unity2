# Unity2
## 1、简答并用程序验证
+ 游戏对象运动的本质是什么？

  + 游戏对象位置的改变

            void Update (){
            this.transform.position += Vector3.left * Time.deltaTime;
            }

+ 请用三种方法以上方法，实现物体的抛物线运动。

  + 1、
  
            void Update () {
                float x = Time.time;
                this.transform.position = new Vector3(x*x,x,0);
	        }
 + 2、

            void Update (){
                this.transform.Translate(Vector3.left * Time.deltaTime);
                this.transform.Translate(Time.deltaTime * Time.deltaTime / 2 * Vector3.down) ;
            }
        
 + 3、
  
            void Start(){
                Rigidbody rb;
                rb = GetComponent<Rigidbody>();
                rigidbody.velocity =new Vector3 (3, 10, 0);
            }


+ 写一个程序，实现一个完整的太阳系， 其他星球围绕太阳的转速必须不一样，且不在一个法平面上。

    + 在每个星球上都有属于自己的类来决定自身的法平面和旋转速度，详情见Solor System文件夹里。

## 2、编程实践
+ 参考了老师提供给我们的代码，在此基础上拓展了两个功能：
  + 游戏结束后不接受用户事件
  + 可以自定义牧师与恶魔的数量
  
+ 玩家动作表：

|用户事件|游戏动作|
|---|---|
|点击牧师或恶魔|牧师或恶魔移动|
|点击船|船移动|
|点击restart按钮|游戏重启|
|修改牧师、恶魔数量|牧师、恶魔数量更改|

+UML类图
![uml](https://github.com/SO4P/Unity2/blob/master/Assets/Materials/2.1.png)
