using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class test2 : MonoBehaviour
{
    [SerializeField] CursorSelect cursorSelectCS;
    [SerializeField] Turn1 TurnCS;
    [SerializeField] Timer TimerCS;
    [SerializeField] Combo ComboCS;
    [SerializeField] Z_Explosion ExplosionCS;
    [SerializeField] Pause PauseCS;

    const int mainPanel = 30;    //メインパネルの数
    const int sidePanel = 42;    //サイドパネルの数
    const int nextPanel = 10;    //ネクストパネルの数
    const int width = 7;   //横の数
    const int height = 6;    //縦の数

    int[] mainNumber = new int[mainPanel];     //3*3のナンバー
    int[] sideNumber = new int[sidePanel];     //4*4のナンバー
    int tmpNumber;          //数字入れ替え時の一時保存
    int[] bonusLevel = new int[sidePanel]; //サイドパネルのボーナス確認 (0=なし,1=あり)
    bool[] bonusFlg = new bool[sidePanel]; //このターン既にボーナスパネルになったかどうか
    int tmpBonus;         //ボーナス入れ替え時の一時保存
    int judgNum = 0;  //和を計算する配列
    public static int score;      //スコア
    public static int turn;      //ターン

    int chooseMain = -1; //現在選んでいるメインナンバー

    bool isHorizontal;     //十字キーの左右入力がニュートラルにもどったか
    bool isVertical;    //十字キーの上下入力がニュートラルにもどったか

    //[SerializeField] Image[] sideImage; //サイドスフィアをいれる
    GameObject[] sideSphere = new GameObject[sidePanel]; //サイドスフィアをいれる
    Color[] sideSphereColor = new Color[sidePanel];  //マテリアル色を変えるための仮入れ
    Color tmpSideColor;
    GameObject tmpObj;
    Renderer[] sideSphereRenderer = new Renderer[sidePanel];    //実際にオブジェクトの色を変更する
    //[SerializeField] GameObject[] obj;  //アニメーションさせるためのオブジェクト一時消し
    PanelAnim[] panelAnim = new PanelAnim[sidePanel];
    PanelAnim tmpAnim;

    //GameObject tmpObj;
    //[SerializeField] Image[] sideBonusFrame;一時消し
    GameObject[] mainSphere = new GameObject[mainPanel];
    Color[] mainSphereColor = new Color[mainPanel];  //マテリアル色を変えるための仮入れ
    Color tmpMainColor;
    Renderer[] mainSphereRenderer = new Renderer[mainPanel];    //実際にオブジェクトの色を変更する

    GameObject[] nextSphere = new GameObject[nextPanel]; //ネクストスフィアをいれる


    int[] mainColorNumber = { 4, 32, 128, 512 };    //メイン色の配列(赤、青、緑、黄)
    int[] sideColorNumber = { 1, 8, 32, 128 };     //サイド色の配列(赤、青、緑、黄)
    int[] rainbowNumber = { 0, 1, width, width + 1 };           //虹衛星を出すときに使う
    bool rainbow;
    int[] rainbowRand = new int[mainPanel]; //虹衛星をランダムに配置するための変数
    int rainbowTarget = 0;
    //[SerializeField] GameObject selectMainImage; //現在選択しているメインパネルを表示

    [SerializeField] GameObject Score;  //スコアのテキストオブジェクト
    Text scoreText;
    int addScoreCount;
    int lossScoreCount;
    int[] addOrLoss = new int[mainPanel];
    int[] targetNum = new int[4];
    [SerializeField] Text[] targetText = new Text[4];
    [SerializeField] GameObject gameClear;
    [SerializeField] GameObject gameOver;
    //[SerializeField] GameObject Timer;  //制限時間のテキストオブジェクト
    //Text timerText;
    //float timeCount = 60.0f;            //制限時間
    //[SerializeField] GameObject Turn;   //ターンのテキストオブジェクト
    //Text turnText;
    //int nowTurn = 0;

    float alpha_Time = 0f;   //点滅させる時間
    float alpha_Sin;    //消すときに点滅させる
    bool alpha_Flg;
    int check = 0; //中身を順にみる変数

    bool[] flgCheck = new bool[mainPanel + 1];  //ポイントになった箇所を記憶,5はnull
    int mainColorNum = 0;               //全パネルが同じ色になったら色を変える
    int checkColorNum;

    bool[] panelMove = new bool[2]; //右か左にパネル移動させるフラグ
                                    //float changeTime = 0.1f;
    int b_t;
    public Material[] _material;           // 割り当てるマテリアル.
    public Texture NormalmapTexture;
    float bravo_Time = 0f;

    [SerializeField] GameObject main;
    [SerializeField] GameObject side;
    [SerializeField] bool[] isPlanet = new bool[mainPanel];

    int[,] satelmap = new int[6, 7] { {  0,  1,  1,  1,  8, 32, 32 }, 
                                      {  0,  1,  8,  8,  8,128, 32 }, 
                                      {  0, 32, 32,  8,128,128, 32 },
                                      {  0,128, 32, 32,  1,128,128 }, 
                                      {  0,128, 32,  1,  1,  1,  8 },
                                      {  0,128,128,  1,  8,  8,  8 } };

    int[,] nextSatel = new int[10, 2] { {  8,   8 }, {   8,  1 },
                                        {  8,   1 }, {   1,  1 },
                                        { 32,  32 }, {  32,128 },
                                        { 32, 128 }, { 128,128 },
                                        { 32, 128 }, { 128, 32 } };


                                        //{ 32, 128 }, { 128, 32 },
                                        //{ 32, 128 }, { 128, 32 },
                                        //{ 32, 128 }, { 128, 32 },
                                        //{ 32, 128 }, { 128, 32 },
                                        //{ 32, 128 }, { 128, 32 } };
public static string oldSceneName;  //リザルトから戻る用

    // Start is called before the first frame update
    void Start()
    {

        //for (int h = 0; h < 6; h++) 
        //{
        //    for(int w = 0; w < 7; w++)
        //    {
        //        satelmap[h, w] = sideColorNumber[2];
        //    }


        //}

        for(int i = 0; i < 4; i++)
        {
            mainSphere[i] = Instantiate(main, new Vector3(7.7f, 3.5f + (-1.5f * i), 0), Quaternion.identity);
            mainNumber[i] = mainColorNumber[i];
            targetNum[i] = 3;
            targetText[i].text = "x " + targetNum[i];
        }

        for (int i = 0; i < mainPanel; i++)
        {
            if (!isPlanet[i])
            {
                //プレハブを元に、インスタンスを生成
                //mainSphere[i] = Instantiate(main, new Vector3(-6 + (2 * (i % (width - 1))), 4 - (2 * (i / (width - 1))), 0.0f), Quaternion.identity);草
                //mainNumber[i] = mainColorNumber[Random.Range(0, 2)];草

                if (chooseMain < 0) chooseMain = i;

                //sideSphere[(i / (width - 1)) + i] = Instantiate(side, new Vector3(-7 + (2 * (i % (width - 1))), 5 - (2 * (i / (width - 1))), 0.0f), Quaternion.identity);
                //sideNumber[(i / (width - 1)) + i] = sideColorNumber[Random.Range(0, 2)];
                //panelAnim[(i / (width - 1)) + i] = sideSphere[(i / (width - 1)) + i].GetComponent<PanelAnim>();

                //if (i >= (mainPanel - 1) - width)
                //{
                //    sideSphere[(i / (width - 1)) + i + width] = Instantiate(side, new Vector3(-7 + (2 * (i % (width - 1))), 3 - (2 * (i / (width - 1))), 0.0f), Quaternion.identity);
                //    sideNumber[(i / (width - 1)) + i + width] = sideColorNumber[Random.Range(0, 2)];
                //    panelAnim[(i / (width - 1)) + i + width] = sideSphere[(i / (width - 1)) + i + width].GetComponent<PanelAnim>();

                //    if (i == mainPanel - 1)
                //    {
                //        sideSphere[(i / (width - 1)) + i + width + 1] = Instantiate(side, new Vector3(-5 + (2 * (i % (width - 1))), 3 - (2 * (i / (width - 1))), 0.0f), Quaternion.identity);
                //        sideNumber[(i / (width - 1)) + i + width + 1] = sideColorNumber[Random.Range(0, 2)];
                //        panelAnim[(i / (width - 1)) + i + width + 1] = sideSphere[(i / (width - 1)) + i + width + 1].GetComponent<PanelAnim>();
                //    }
                //}

                //if (i % 6 == 5)    //右端の列で追加2つ生成
                //{
                //    sideSphere[(i / (width - 1)) + i + 1] = Instantiate(side, new Vector3(-5 + (2 * (i % (width - 1))), 5 - (2 * (i / (width - 1))), 0.0f), Quaternion.identity);
                //    sideNumber[(i / (width - 1)) + i + 1] = sideColorNumber[Random.Range(0, 2)];
                //    panelAnim[(i / (width - 1)) + i + 1] = sideSphere[(i / (width - 1)) + i + 1].GetComponent<PanelAnim>();
                //}
                ////mainSphereColor[i] = mainSphere[i].GetComponent<Renderer>().material.color;
            }
        }

        for (int i = 0; i < sidePanel; i++) //衛星の生成
        {
            if (i <= 6)  //最上行のとき
            {
                if (i == 0 && isPlanet[0]) //隅の惑星がなければ隅の衛星も作らない
                {
                }
                else if (i == 6 && isPlanet[5]) //隅の惑星がなければ隅の衛星も作らない
                {
                }
                else if (i != 0 && i != 6 && isPlanet[i - (i / width) - 1] && isPlanet[i - (i / width)])//惑星1列目を見て、両隣がなければ作らない
                {
                }
                else
                {
                    //プレハブを元に、インスタンスを生成、
                    sideSphere[i] = Instantiate(side, new Vector3(-5 + (2 * (i % width - 1)), 5 - (2 * (i / width)), 0.0f), Quaternion.identity);
                    panelAnim[i] = sideSphere[i].GetComponent<PanelAnim>();
                    sideNumber[i] = sideColorNumber[Random.Range(0, 4)];
                    
                }
            }
            else if (i > 0 && i % 7 == 0) //最左列のとき
            {
                if (i == 35 && isPlanet[24]) //隅の惑星がなければ隅の衛星も作らない
                {

                }
                else if (i != 35 && isPlanet[i - (i / width) - 6] && isPlanet[i - (i / width)]) //惑星最左列を見て、両隣がなければ作らない
                {

                }
                else
                {
                    //プレハブを元に、インスタンスを生成、
                    sideSphere[i] = Instantiate(side, new Vector3(-5 + (2 * (i % width - 1)), 5 - (2 * (i / width)), 0.0f), Quaternion.identity);
                    panelAnim[i] = sideSphere[i].GetComponent<PanelAnim>();
                    sideNumber[i] = sideColorNumber[Random.Range(0, 4)];
                    
                }
            }
            else if (i > 6 && i % 7 == 6)   //最右列のとき
            {
                if (i == 41 && isPlanet[29])//隅の惑星がなければ隅の衛星も作らない
                {

                }
                else if (i != 41 && isPlanet[i - (i / width) - width] && isPlanet[i - (i / width) - 1]) //惑星最右列を見て、両隣がなければ作らない
                {

                }
                else
                {
                    //プレハブを元に、インスタンスを生成、
                    sideSphere[i] = Instantiate(side, new Vector3(-5 + (2 * (i % width - 1)), 5 - (2 * (i / width)), 0.0f), Quaternion.identity);
                    panelAnim[i] = sideSphere[i].GetComponent<PanelAnim>();
                    sideNumber[i] = sideColorNumber[Random.Range(0, 4)];
                 
                }
            }
            else if (i > 35 && i < 41)  //最下行の時
            {
                if (isPlanet[i - (i / width) - width] && isPlanet[i - (i / width) - (width - 1)])//惑星最下列を見て、両隣がなければ作らない
                {

                }
                else
                {
                    //プレハブを元に、インスタンスを生成、
                    sideSphere[i] = Instantiate(side, new Vector3(-5 + (2 * (i % width - 1)), 5 - (2 * (i / width)), 0.0f), Quaternion.identity);
                    panelAnim[i] = sideSphere[i].GetComponent<PanelAnim>();
                    sideNumber[i] = sideColorNumber[Random.Range(0, 4)];

                }
            }
            else
            {
                if (isPlanet[i - (i / width) - width] && isPlanet[i - (i / width) - (width - 1)] && isPlanet[i - (i / width) - 1] && isPlanet[i - (i / width)])//四方に惑星がない場合は衛星を作らない
                {

                }
                else
                {
                    //プレハブを元に、インスタンスを生成、
                    sideSphere[i] = Instantiate(side, new Vector3(-5 + (2 * (i % width - 1)), 5 - (2 * (i / width)), 0.0f), Quaternion.identity);
                    panelAnim[i] = sideSphere[i].GetComponent<PanelAnim>();
                    sideNumber[i] = sideColorNumber[Random.Range(0, 4)];

                }
            }

            //sideSphere[(i / (width - 1)) + i] = Instantiate(side, new Vector3(-7 + (2 * (i % (width - 1))), 5 - (2 * (i / (width - 1))), 0.0f), Quaternion.identity);
            //sideSphere[(i / (width - 1)) + i + width] = Instantiate(side, new Vector3(-5 + (2 * (i % (width - 1))), 5 - (2 * (i / (width - 1))), 0.0f), Quaternion.identity);
            //sideSphere[(i / (width - 1)) + i + width + 1] = Instantiate(side, new Vector3(-7 + (2 * (i % (width - 1))), 3 - (2 * (i / (width - 1))), 0.0f), Quaternion.identity);
            //sideSphere[(i / (width - 1)) + i + 1] = Instantiate(side, new Vector3(-5 + (2 * (i % (width - 1))), 3 - (2 * (i / (width - 1))), 0.0f), Quaternion.identity);
        }


        for (int i = 0; i < sidePanel; i++)
        {
            if (i%7!=0) {
                if (i < 7) sideNumber[i] = satelmap[0, i];
                else if (i < 14) sideNumber[i] = satelmap[1, i % 7];
                else if (i < 21) sideNumber[i] = satelmap[2, i % 14];
                else if (i < 28) sideNumber[i] = satelmap[3, i % 21];
                else if (i < 35) sideNumber[i] = satelmap[4, i % 28];
                else if (i < 42) sideNumber[i] = satelmap[5, i % 35];
            }
        }
        //sideNumber[7] = satelmap[0, 0];
        //Debug.Log(sideNumber[7]);

        scoreText = Score.GetComponent<Text>();
        //timerText = Timer.GetComponent<Text>();
        //turnText = Turn.GetComponent<Text>();

        ColorChange();   //パネルの色変更
        TurnCS.nowTurn = 0; //ターン数の指定
        TimerCS.maxTime = 60.0f;
        TimerCS.timeCount = TimerCS.maxTime;
        score = 0;  //スコアの初期化
    }

    // Update is called once per frame
    void Update()
    {
        if (!PauseCS.isPause)
        {
            if (!alpha_Flg)
            {
                if (!panelMove[0] && !panelMove[1]) PanelOperation();   //パネルの操作
                else if (panelMove[0] || panelMove[1]) PanelMove();        //パネルのアニメーション
                PointCheck();             //盤面が揃ったか見る 揃ったらすぐ変わる
                //PointBlinking();            //4つ揃ったときの点滅
                //ColorChange();              //パネルの色変更
                TimerCS.TimerCount();       //制限時間のカウントと表示
                TurnEnd();                  //ターン終了時の処理

                //SelectImageMove();  //現在選んでいるパネルの可視化 ここで呼ぶ
                cursorSelectCS.SelectImageMove(chooseMain);

            }
            else if (alpha_Flg) alpha();

            //ゲーム終了
            //        if (Input.GetButtonDown("Start"))   //Yを押したら終了
            //        {
            //#if UNITY_EDITOR
            //            UnityEditor.EditorApplication.isPlaying = false;
            //#else
            //    Application.Quit();
            //#endif
            //        }
        }
    }

    //***
    void alpha()
    {
        if ((flgCheck[check] && check <= (mainPanel - 1)) && !isPlanet[check])   //0~9で条件を満たしたら
        {

            if (alpha_Time < 0.6f)    //条件を満たしたパネルの点滅
            {
                alpha_Time += Time.deltaTime * 2;    //制限時間のカウントダウン
                alpha_Sin = (alpha_Time * 6) + 5;
                //alpha_Sin = Mathf.Sin(Time.time) / 2 + 0.5f;

                //mainSphere[check].GetComponent<Renderer>().material.SetFloat("_AtmosphereDensity", alpha_Sin);　草
                sideSphere[(check / (width - 1)) + check].GetComponent<Renderer>().material.SetFloat("_AtmosphereDensity", alpha_Sin);
                sideSphere[(check / (width - 1)) + check + 1].GetComponent<Renderer>().material.SetFloat("_AtmosphereDensity", alpha_Sin);
                sideSphere[(check / (width - 1)) + check + width].GetComponent<Renderer>().material.SetFloat("_AtmosphereDensity", alpha_Sin);
                sideSphere[(check / (width - 1)) + check + width + 1].GetComponent<Renderer>().material.SetFloat("_AtmosphereDensity", alpha_Sin);
            }
            else if (alpha_Time >= 0.6f && alpha_Time <= 0.7f)
            {
                alpha_Time = 0.8f;
                //flgCheck[check] = false;
                //ColorChange();

                //mainNumber[i] = mainColorNumber[0];
                sideNumber[(check / (width - 1)) + check] = sideColorNumber[Random.Range(0, 4)];
                sideNumber[(check / (width - 1)) + check + 1] = sideColorNumber[Random.Range(0, 4)];
                sideNumber[(check / (width - 1)) + check + width + 1] = sideColorNumber[Random.Range(0, 4)];
                sideNumber[(check / (width - 1)) + check + width] = sideColorNumber[Random.Range(0, 4)];

                sideNumber[(check / (width - 1)) + check] = nextSatel[0, 0];
                sideNumber[(check / (width - 1)) + check + width] = nextSatel[0, 1];
                sideNumber[(check / (width - 1)) + check + 1] = nextSatel[1, 0];
                sideNumber[(check / (width - 1)) + check + width + 1]=nextSatel[1, 1];

                for (int i = 0; i <= 6; i += 2)
                {
                    nextSatel[i, 0] = nextSatel[i + 2, 0];
                    nextSatel[i, 1] = nextSatel[i + 2, 1];
                    nextSatel[i + 1, 0] = nextSatel[i + 2 + 1, 0];
                    nextSatel[i + 1, 1] = nextSatel[i + 2 + 1, 1];
                    if (i == 6)
                    {
                        nextSatel[8, 0] = sideColorNumber[Random.Range(0, 4)];
                        nextSatel[8, 1] = sideColorNumber[Random.Range(0, 4)];
                        nextSatel[9, 0] = sideColorNumber[Random.Range(0, 4)];
                        nextSatel[9, 1] = sideColorNumber[Random.Range(0, 4)];
                    }
                }

                    //mainColorNum += mainNumber[check];  //[0]^[3]合計を得る草

                    rainbowRand[rainbowTarget] = check; //条件を満たした惑星の位置を把握しておく
                rainbowTarget += 1;

                //if (isAddScore)
                //{
                //    ScoreAdd();

                //    //ランダムな数値にいれかえ
                //    MainGenerate();
                //}
                //if (isLossScore) ScoreLoss();

                if (addOrLoss[check] == 1)
                {
                    ScoreAdd();
                    //ランダムな数値にいれかえ
                    //MainGenerate();

                    addOrLoss[check] = 9;
                }
                if (addOrLoss[check] == 0)
                {
                    ScoreAdd();
                    //ScoreLoss();
                    addOrLoss[check] = 9;
                }

                ////ランダムな数値にいれかえ
                //MainGenerate();

                b_t = 1;

                
            }
            else if (b_t<5 && b_t>0)
            {
                bravo_Time += Time.deltaTime*3;
                if (b_t == 4 && bravo_Time > 1.5f)
                {
                    ExplosionCS.particle[(check / (width - 1)) + check + width + 1].Play(); //条件を満たした惑星が爆発
                    b_t = 0;
                    alpha_Time = 0f;
                    bravo_Time = 0f;
                    ExplosionCS.audio.PlayOneShot(ExplosionCS.clip);//爆発のSEを再生
                    check += 1;
                }
                if (b_t == 3 && bravo_Time > 1.0f)
                {
                    ExplosionCS.particle[(check / (width - 1)) + check + width].Play(); //条件を満たした惑星が爆発
                    ExplosionCS.audio.PlayOneShot(ExplosionCS.clip);//爆発のSEを再生
                    b_t = 4;
                }
                if (b_t == 2 && bravo_Time > 0.5f)
                {
                    ExplosionCS.particle[(check / (width - 1)) + check + 1].Play(); //条件を満たした惑星が爆発
                    ExplosionCS.audio.PlayOneShot(ExplosionCS.clip);//爆発のSEを再生
                    b_t = 3;
                }
                if (b_t == 1 && bravo_Time > 0.0f)
                {
                    ExplosionCS.particle[(check / (width - 1)) + check].Play(); //条件を満たした惑星が爆発
                    ExplosionCS.audio.PlayOneShot(ExplosionCS.clip);//爆発のSEを再生
                    b_t = 2;
                }
                
                Invoke("ClearCheck", 0.2f); //クリア条件を満たしたかチェック
                ColorChange();   //パネルの色変更
                
            }
            
        }
        else if (check > (mainPanel - 1))    //最後に盤面を変える
        {
            if (alpha_Sin <= 8)    //半透明から透明へ
            {
                alpha_Sin += Time.deltaTime * 2;

                for (int i = 0; i < mainPanel; i++)
                {
                    if (flgCheck[i])
                    {
                        //mainSphere[i].GetComponent<Renderer>().material.SetFloat("_AtmosphereDensity", 8);　草
                        sideSphere[(i / (width - 1)) + i].GetComponent<Renderer>().material.SetFloat("_AtmosphereDensity", 8);
                        sideSphere[(i / (width - 1)) + i + 1].GetComponent<Renderer>().material.SetFloat("_AtmosphereDensity", 8);
                        sideSphere[(i / (width - 1)) + i + width].GetComponent<Renderer>().material.SetFloat("_AtmosphereDensity", 8);
                        sideSphere[(i / (width - 1)) + i + width + 1].GetComponent<Renderer>().material.SetFloat("_AtmosphereDensity", 8);
                    }
                }
            }
            else
            {

                //for (int i = 0; i < mainPanel; i++)
                //{
                //    if (flgCheck[i])
                //    {

                //        //ランダムな数値にいれかえ
                //        mainNumber[i] = mainColorNumber[Random.Range(0, 2)];
                //        //mainNumber[i] = mainColorNumber[0];
                //        sideNumber[(i / 3) + i] = sideColorNumber[Random.Range(0, 2)];
                //        sideNumber[(i / 3) + i + 1] = sideColorNumber[Random.Range(0, 2)];
                //        sideNumber[(i / 3) + i + 5] = sideColorNumber[Random.Range(0, 2)];
                //        sideNumber[(i / 3) + i + 4] = sideColorNumber[Random.Range(0, 2)];

                //        rainbowRand[rainbowTarget] = i; //条件を満たした惑星の位置を把握しておく
                //        rainbowTarget += 1;
                //    }
                //    mainColorNum += mainNumber[i];  //[0]^[3]合計を得る
                //}

                //if (ComboCS.comboCount >= 3 && !rainbow) //条件を満たした惑星のどこかに3コンボ以上で虹惑星を１つだす 草
                //{
                //    int i = rainbowRand[Random.Range(0, ComboCS.comboCount)];
                //    rainbow = true;
                //    sideNumber[i / (width - 1) + i + rainbowNumber[Random.Range(0, 4)]] = sideColorNumber[2];
                //}

                //for (int j = 0; j < 4; j++)  //[0]^[9]の合計と色*4を見る { 4, 32, 128, 512}草
                //{
                //    while (mainColorNum == (mainColorNumber[j] * mainPanel))  //9色同じだったら処理を繰り返す
                //    {
                //        mainColorNum = 0;   //一度numを0にし
                //        for (int f = 0; f < mainPanel; f++)
                //        {
                //            if (flgCheck[f]) mainNumber[f] = mainColorNumber[Random.Range(0, 2)]; //消したマスをランダムな色に変えて
                //            mainColorNum += mainNumber[f];       //もう一度[0]^[9]の合計を得る
                //        }
                //    }
                //}
                Bonus();    //ボーナスパネル設定
                for (int f = 0; f < mainPanel; f++) flgCheck[f] = false; //念のため別のforでfalseにする
                mainColorNum = 0;
                ColorChange();   //パネルの色変更
                check = 0;
                rainbow = false;
                alpha_Flg = false;
                rainbowTarget = 0;
            }
        }
        else check += 1;
    }
    //***
    void PanelOperation()
    {
        turn = TurnCS.nowTurn;
        //パネル反時計回り
        if (Input.GetButtonDown("LB"))
        {
            panelMove[0] = true;
            if (!TimerCS.countStart) TimerCS.countStart = true;
        }
        //パネル時計回り
        else if (Input.GetButtonDown("RB"))
        {
            panelMove[1] = true;
            if (!TimerCS.countStart) TimerCS.countStart = true;
        }

        //十字キーのパネル選択
        if (0 > Input.GetAxis("ClossVertical") && !isVertical)    //↓入力時
        {
            do
            {
                if (chooseMain >= 24) chooseMain -= 24;
                else chooseMain += 6;
            } while (isPlanet[chooseMain]);
            isVertical = true;
        }
        if (0 < Input.GetAxis("ClossVertical") && !isVertical)  //↑入力時
        {
            do
            {
                if (chooseMain <= 5) chooseMain += 24;
                else chooseMain -= 6;
            } while (isPlanet[chooseMain]);
            isVertical = true;
        }
        if (0 > Input.GetAxis("ClossHorizontal") && !isHorizontal)  //←入力時
        {
            do
            {
                if (chooseMain % (width - 1) == 0) chooseMain += 5;
                else chooseMain -= 1;
            } while (isPlanet[chooseMain]);
            isHorizontal = true;
        }
        if (0 < Input.GetAxis("ClossHorizontal") && !isHorizontal)    //→入力時
        {
            do
            {
                if (chooseMain % (width - 1) == 5) chooseMain -= 5;
                else chooseMain += 1;
            } while (isPlanet[chooseMain]);
            isHorizontal = true;
        }

        if (0 == Input.GetAxis("ClossHorizontal")) isHorizontal = false;
        if (0 == Input.GetAxis("ClossVertical")) isVertical = false;
    }
    //***
    void PointCheck()
    {

        for (int i = 0; i < mainPanel; i++)
        {
            if (!isPlanet[i])
            {
                //judgNum += sideNumber[(i / 3) + i];
                //judgNum += sideNumber[(i / 3) + i + 1];
                //judgNum += sideNumber[(i / 3) + i + 5];
                //judgNum += sideNumber[(i / 3) + i + 4];

                judgNum = mainNumber[0];

                //if (judgNum == sideNumber[(i / (width - 1)) + i] * 4)
                //    if (judgNum == sideNumber[(i / (width - 1)) + i + 1] * 4)
                //        if (judgNum == sideNumber[(i / (width - 1)) + i + width + 1] * 4)
                //            if (judgNum == sideNumber[(i / (width - 1)) + i + width] * 4) //色を満たした
                //            {
                //                flgCheck[i] = true;

                //                //ボーナスフラグon
                //                bonusFlg[(i / (width - 1)) + i] = true;
                //                bonusFlg[(i / (width - 1)) + i + 1] = true;
                //                bonusFlg[(i / (width - 1)) + i + width + 1] = true;
                //                bonusFlg[(i / (width - 1)) + i + width] = true;

                //                alpha_Flg = true;
                //            }

                if (sideNumber[(i / (width - 1)) + i] * 4 == sideNumber[(i / (width - 1)) + i + 1] * 4)
                    if (sideNumber[(i / (width - 1)) + i + 1] * 4 == sideNumber[(i / (width - 1)) + i + width + 1] * 4)
                        if (sideNumber[(i / (width - 1)) + i + width + 1] * 4 == sideNumber[(i / (width - 1)) + i + width] * 4)
                        {
                            if (mainNumber[0] == sideNumber[(i / (width - 1)) + i + width] * 4) //色を満たした
                            {
                                flgCheck[i] = true;
                                alpha_Flg = true;
                                addScoreCount += 1;
                                addOrLoss[i] = 1;
                                if (targetNum[0] > 0) targetNum[0] -= 1;
                                targetText[0].text =  "x " + targetNum[0];
                            }
                            else if (mainNumber[1] == sideNumber[(i / (width - 1)) + i + width] * 4) //色を満たした
                            {
                                flgCheck[i] = true;
                                alpha_Flg = true;
                                lossScoreCount += 1;
                                addOrLoss[i] = 0;
                                if (targetNum[1] > 0) targetNum[1] -= 1;
                                targetText[1].text = "x " + targetNum[1];
                            }
                            else if (mainNumber[2] == sideNumber[(i / (width - 1)) + i + width] * 4) //色を満たした
                            {
                                flgCheck[i] = true;
                                alpha_Flg = true;
                                lossScoreCount += 1;
                                addOrLoss[i] = 0;
                                if (targetNum[2] > 0) targetNum[2] -= 1;
                                targetText[2].text = "x " + targetNum[2];
                            }
                            else if (mainNumber[3] == sideNumber[(i / (width - 1)) + i + width] * 4) //色を満たした
                            {
                                flgCheck[i] = true;
                                alpha_Flg = true;
                                lossScoreCount += 1;
                                addOrLoss[i] = 0;
                                if (targetNum[3] > 0) targetNum[3] -= 1;
                                targetText[3].text = "x " + targetNum[3];
                            }
                        }

                judgNum = 0;

                //if (judgNum == mainNumber[i])   //色を満たした
                //{
                //    flgCheck[i] = true;

                //    //ボーナスフラグon
                //    bonusFlg[(i / 3) + i] = true;
                //    bonusFlg[(i / 3) + i + 1] = true;
                //    bonusFlg[(i / 3) + i + 5] = true;
                //    bonusFlg[(i / 3) + i + 4] = true;

                //    alpha_Flg = true;
                //}
            }
        }
    }

    //***
    public void ColorChange()
    {
        for (int i = 0; i < 4; i++)
        {
            switch (mainNumber[i])
            {
                case 4:
                    //mainSphereColor[i] = Color.red;
                    //mainSphere[i].GetComponent<Renderer>().material.color = mainSphereColor[i];
                    mainSphere[i].GetComponent<Renderer>().material = _material[0];
                    break;
                case 32:
                    //mainSphereColor[i] = Color.blue;
                    //mainSphere[i].GetComponent<Renderer>().material.color = mainSphereColor[i];
                    mainSphere[i].GetComponent<Renderer>().material = _material[1];
                    break;
                case 128:
                    //mainSphereColor[i] = Color.yellow;
                    //mainSphere[i].GetComponent<Renderer>().material.color = mainSphereColor[i];
                    mainSphere[i].GetComponent<Renderer>().material = _material[2];
                    break;
                case 512:
                    //mainSphereColor[i] = Color.green;
                    //mainSphere[i].GetComponent<Renderer>().material.color = mainSphereColor[i];
                    mainSphere[i].GetComponent<Renderer>().material = _material[3];
                    break;
                default:
                    break;
            }
        }
        //switch (mainNumber[0])
        //{
        //    case 4:
        //        //mainSphereColor[i] = Color.red;
        //        //mainSphere[i].GetComponent<Renderer>().material.color = mainSphereColor[i];
        //        //mainSphere[i].GetComponent<Renderer>().material = _material[3];
        //        mainSphere[0].GetComponent<Renderer>().material = _material[0];
        //        break;
        //    case 32:
        //        //mainSphereColor[i] = Color.blue;
        //        //mainSphere[i].GetComponent<Renderer>().material.color = mainSphereColor[i];
        //        //mainSphere[i].GetComponent<Renderer>().material = _material[4];
        //        mainSphere[0].GetComponent<Renderer>().material = _material[1];
        //        break;
        //    case 128:
        //        //mainSphereColor[i] = Color.yellow;
        //        //mainSphere[i].GetComponent<Renderer>().material.color = mainSphereColor[i];
        //        mainSphere[0].GetComponent<Renderer>().material = _material[2];
        //        break;
        //    case 512:
        //        //mainSphereColor[i] = Color.green;
        //        //mainSphere[i].GetComponent<Renderer>().material.color = mainSphereColor[i];
        //        mainSphere[0].GetComponent<Renderer>().material = _material[3];
        //        break;
        //    default:
        //        break;
    //}

        for (int i = 0; i < sidePanel; i++)
        {
            switch (sideNumber[i])
            {
                case 1:
                    //sideSphereColor[i] = Color.red;
                    //sideSphere[i].GetComponent<Renderer>().material.color = sideSphereColor[i];
                    sideSphere[i].GetComponent<Renderer>().material = _material[0];
                    break;
                case 8:
                    //sideSphereColor[i] = Color.blue;
                    //sideSphere[i].GetComponent<Renderer>().material.color = sideSphereColor[i];
                    sideSphere[i].GetComponent<Renderer>().material = _material[1];
                    break;
                case 32:
                    //sideSphereColor[i] = Color.yellow;
                    //sideSphere[i].GetComponent<Renderer>().material.color = sideSphereColor[i];
                    sideSphere[i].GetComponent<Renderer>().material = _material[2];
                    break;
                case 128:
                    //sideSphereColor[i] = Color.green;
                    //sideSphere[i].GetComponent<Renderer>().material.color = sideSphereColor[i];
                    sideSphere[i].GetComponent<Renderer>().material = _material[3];
                    break;
                default:
                    break;
            }

            //一時消
            //if (bonusLevel[i] == 0) sideBonusFrame[i].color = Color.gray; //ボーナスがなければ灰縁
            //else if (bonusLevel[i] > 0) sideBonusFrame[i].color = Color.yellow;   //ボーナスがあれば金縁
        }
    }
    //***
    void Bonus()
    {
        for (int f = 0; f < sidePanel; f++)
        {
            //ボーナスフラグがあれば+1なければ-1(下限0)
            if (bonusLevel[f] > 0 && !bonusFlg[f]) bonusLevel[f] -= 1;
            if (bonusFlg[f]) bonusLevel[f] = 1;

            //if (bonusLevel[f] == 0) sideBonusFrame[f].color = Color.gray;
            //else if(bonusLevel[f] > 0) sideBonusFrame[f].color = Color.yellow;

            bonusFlg[f] = false;
        }
    }
    //***
    void PanelMove()    //アニメーション終了時に呼ぶ
    {
        //if (changeTime > 0) changeTime -= Time.deltaTime;
        //else if (changeTime <= 0)
        //{
        if (panelMove[0])   //反時計周り
        {
            //パネルの回転アニメーション
            panelAnim[(chooseMain / (width - 1)) + chooseMain].animFlg[1] = true; //down
            panelAnim[(chooseMain / (width - 1)) + chooseMain + 1].animFlg[2] = true; //left
            panelAnim[(chooseMain / (width - 1)) + chooseMain + width + 1].animFlg[3] = true; //up
            panelAnim[(chooseMain / (width - 1)) + chooseMain + width].animFlg[0] = true; //right

            //ナンバー入れ替え
            tmpNumber = sideNumber[(chooseMain / (width - 1)) + chooseMain];
            sideNumber[(chooseMain / (width - 1)) + chooseMain] = sideNumber[(chooseMain / (width - 1)) + chooseMain + 1];
            sideNumber[(chooseMain / (width - 1)) + chooseMain + 1] = sideNumber[(chooseMain / (width - 1)) + chooseMain + width + 1];
            sideNumber[(chooseMain / (width - 1)) + chooseMain + width + 1] = sideNumber[(chooseMain / (width - 1)) + chooseMain + width];
            sideNumber[(chooseMain / (width - 1)) + chooseMain + width] = tmpNumber;

            //ボーナス入れ替え
            tmpBonus = bonusLevel[(chooseMain / (width - 1)) + chooseMain];
            bonusLevel[(chooseMain / (width - 1)) + chooseMain] = bonusLevel[(chooseMain / (width - 1)) + chooseMain + 1];
            bonusLevel[(chooseMain / (width - 1)) + chooseMain + 1] = bonusLevel[(chooseMain / (width - 1)) + chooseMain + width + 1];
            bonusLevel[(chooseMain / (width - 1)) + chooseMain + width + 1] = bonusLevel[(chooseMain / (width - 1)) + chooseMain + width];
            bonusLevel[(chooseMain / (width - 1)) + chooseMain + width] = tmpBonus;

            //オブジェクト入れ替え
            tmpObj = sideSphere[(chooseMain / (width - 1)) + chooseMain];
            sideSphere[(chooseMain / (width - 1)) + chooseMain] = sideSphere[(chooseMain / (width - 1)) + chooseMain + 1];
            sideSphere[(chooseMain / (width - 1)) + chooseMain + 1] = sideSphere[(chooseMain / (width - 1)) + chooseMain + width + 1];
            sideSphere[(chooseMain / (width - 1)) + chooseMain + width + 1] = sideSphere[(chooseMain / (width - 1)) + chooseMain + width];
            sideSphere[(chooseMain / (width - 1)) + chooseMain + width] = tmpObj;

            //スクリプト入れ替え
            tmpAnim = panelAnim[(chooseMain / (width - 1)) + chooseMain];
            panelAnim[(chooseMain / (width - 1)) + chooseMain] = panelAnim[(chooseMain / (width - 1)) + chooseMain + 1];
            panelAnim[(chooseMain / (width - 1)) + chooseMain + 1] = panelAnim[(chooseMain / (width - 1)) + chooseMain + width + 1];
            panelAnim[(chooseMain / (width - 1)) + chooseMain + width + 1] = panelAnim[(chooseMain / (width - 1)) + chooseMain + width];
            panelAnim[(chooseMain / (width - 1)) + chooseMain + width] = tmpAnim;

            panelMove[0] = false;
        }
        else if (panelMove[1])  //時計周り
        {
            //パネルの回転アニメーション
            panelAnim[(chooseMain / (width - 1)) + chooseMain].animFlg[4] = true; //right2
            panelAnim[(chooseMain / (width - 1)) + chooseMain + 1].animFlg[5] = true; //down2
            panelAnim[(chooseMain / (width - 1)) + chooseMain + width + 1].animFlg[6] = true; //up2
            panelAnim[(chooseMain / (width - 1)) + chooseMain + width].animFlg[7] = true; //left2

            //ナンバー入れ替え
            tmpNumber = sideNumber[(chooseMain / (width - 1)) + chooseMain];
            sideNumber[(chooseMain / (width - 1)) + chooseMain] = sideNumber[(chooseMain / (width - 1)) + chooseMain + width];
            sideNumber[(chooseMain / (width - 1)) + chooseMain + width] = sideNumber[(chooseMain / (width - 1)) + chooseMain + width + 1];
            sideNumber[(chooseMain / (width - 1)) + chooseMain + width + 1] = sideNumber[(chooseMain / (width - 1)) + chooseMain + 1];
            sideNumber[(chooseMain / (width - 1)) + chooseMain + 1] = tmpNumber;

            //ボーナス入れ替え
            tmpBonus = bonusLevel[(chooseMain / (width - 1)) + chooseMain];
            bonusLevel[(chooseMain / (width - 1)) + chooseMain] = bonusLevel[(chooseMain / (width - 1)) + chooseMain + width];
            bonusLevel[(chooseMain / (width - 1)) + chooseMain + width] = bonusLevel[(chooseMain / (width - 1)) + chooseMain + width + 1];
            bonusLevel[(chooseMain / (width - 1)) + chooseMain + width + 1] = bonusLevel[(chooseMain / (width - 1)) + chooseMain + 1];
            bonusLevel[(chooseMain / (width - 1)) + chooseMain + 1] = tmpBonus;

            //オブジェクト入れ替え
            tmpObj = sideSphere[(chooseMain / (width - 1)) + chooseMain];
            sideSphere[(chooseMain / (width - 1)) + chooseMain] = sideSphere[(chooseMain / (width - 1)) + chooseMain + width];
            sideSphere[(chooseMain / (width - 1)) + chooseMain + width] = sideSphere[(chooseMain / (width - 1)) + chooseMain + width + 1];
            sideSphere[(chooseMain / (width - 1)) + chooseMain + width + 1] = sideSphere[(chooseMain / (width - 1)) + chooseMain + 1];
            sideSphere[(chooseMain / (width - 1)) + chooseMain + 1] = tmpObj;

            ////スクリプト入れ替え
            tmpAnim = panelAnim[(chooseMain / (width - 1)) + chooseMain];
            panelAnim[(chooseMain / (width - 1)) + chooseMain] = panelAnim[(chooseMain / (width - 1)) + chooseMain + width];
            panelAnim[(chooseMain / (width - 1)) + chooseMain + width] = panelAnim[(chooseMain / (width - 1)) + chooseMain + width + 1];
            panelAnim[(chooseMain / (width - 1)) + chooseMain + width + 1] = panelAnim[(chooseMain / (width - 1)) + chooseMain + 1];
            panelAnim[(chooseMain / (width - 1)) + chooseMain + 1] = tmpAnim;

            ////色の入れ替え
            //tmpSideColor = sideSphereColor[(chooseMain / 3) + chooseMain];
            //sideSphereColor[(chooseMain / 3) + chooseMain] = sideSphereColor[(chooseMain / 3) + chooseMain + 4];
            //sideSphereColor[(chooseMain / 3) + chooseMain + 4] = sideSphereColor[(chooseMain / 3) + chooseMain + 5];
            //sideSphereColor[(chooseMain / 3) + chooseMain + 5] = sideSphereColor[(chooseMain / 3) + chooseMain + 1];
            //sideSphereColor[(chooseMain / 3) + chooseMain + 1] = tmpSideColor;

            panelMove[1] = false;
            //    }
            //    changeTime = 0.1f;
        }
    }

    void ScoreAdd()
    {
        //スコア+100と各パネルのボーナス分スコア+50
        //score += 100 + (50 * (bonusLevel[(check / 3) + check] + bonusLevel[(check / 3) + check + 1]
        //    + bonusLevel[(check / 3) + check + 5] + bonusLevel[(check / 3) + check + 4]));

        ////スコア100と1コンボ50
        score += 1000 + (50 * ComboCS.comboCount);

        scoreText.text = "" + score;

        ComboCS.comboTime = 5.0f;
        ComboCS.comboCount += 1;
        addScoreCount -= 1;
    }

    void ScoreLoss()
    {
        //スコア100と1コンボ50
        score -= 100;

        scoreText.text = "" + score;

        ComboCS.comboTime = 5.0f;
        ComboCS.comboCount = 0;
        lossScoreCount -= 1;
    }

    void TurnEnd()
    {
        if (TurnCS.nowTurn < 21)
        {
            if ((Input.GetButtonDown("LB") || Input.GetButtonDown("RB") || TimerCS.timeOut) && TimerCS.countStart == true) //Xか制限時間でターン終了
            {
                if (!alpha_Flg) PointCheck();
                //TimerCS.timeCount = 30.0f;
                //TimerCS.timeOut = false;
                //TimerCS.countStart = false;
                //TimerCS.bigTimerText.enabled = false;
                //ComboCS.comboCount = 0; //コンボカウントリセット
                TurnCS.TurnCount();        //経過ターンの更新表示
                Invoke("ClearCheck", 0.2f); //クリア条件を満たしたかチェック
            }
        }
        else
        {
            if (TimerCS.timeCount > 0) TimerCS.timeCount = 0f;
            //リザルト画面へ
            Result();   //リザルトに遷移
        }
       

        //if (TimerCS.timeCount <= 0f) //Xか制限時間でターン終了
        //{
        //    if (TimerCS.timeCount > 0) TimerCS.timeCount = 0f;
        //    Result();   //リザルトに遷移
        //}
    }
    void PointBlinking()
    {
        for (int i = 0; i < mainPanel; i++)
        {
            if (!isPlanet[i])
            {

                judgNum = mainNumber[i];

                if (judgNum == sideNumber[(i / (width - 1)) + i] * 4 || sideNumber[(i / (width - 1)) + i] == 32)
                    if (judgNum == sideNumber[(i / (width - 1)) + i + 1] * 4 || sideNumber[(i / (width - 1)) + i + 1] == 32)
                        if (judgNum == sideNumber[(i / (width - 1)) + i + width + 1] * 4 || sideNumber[(i / (width - 1)) + i + width + 1] == 32)
                            if (judgNum == sideNumber[(i / (width - 1)) + i + width] * 4 || sideNumber[(i / (width - 1)) + i + width] == 32) //色を満たした
                            {
                                float blinking = 0f;
                                float blinkingSpeed = 2.0f;
                                blinking = Mathf.Sin(2 * Mathf.PI * blinkingSpeed * Time.time); //sin波取得 点滅(-1~1)

                                //mainSphere[i].GetComponent<Renderer>().material.SetFloat("_AtmosphereDensity", 6 + blinking);草
                                sideSphere[(i / (width - 1)) + i].GetComponent<Renderer>().material.SetFloat("_AtmosphereDensity", 6 + blinking);
                                sideSphere[(i / (width - 1)) + i + 1].GetComponent<Renderer>().material.SetFloat("_AtmosphereDensity", 6 + blinking);
                                sideSphere[(i / (width - 1)) + i + width].GetComponent<Renderer>().material.SetFloat("_AtmosphereDensity", 6 + blinking);
                                sideSphere[(i / (width - 1)) + i + width + 1].GetComponent<Renderer>().material.SetFloat("_AtmosphereDensity", 6 + blinking);
                                //Debug.Log(blinking);
                            }
                judgNum = 0;
            }
        }
    }

    void ExplosionStop()
    {
        //for (int i = 0; i < mainPanel; i++)
        //{
        //    ExplosionCS.particle[i].Stop();
        //}
    }

    void MainGenerate()
    {

        do
        {
            //ランダムな数値にいれかえ
            mainNumber[0] = mainColorNumber[Random.Range(0, 4)];
            checkColorNum = 0;

            for (int i = 0; i < sidePanel; i++)
            {

                if(sideNumber[i] * 4 == mainNumber[0])
                {
                    checkColorNum += 1;
                }
            }
        } while (checkColorNum < 4);
    }

    void Result()
    {
        //リザルト画面へ
        oldSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("MasterResult");
    }

    void ClearCheck()
    {
        
        if (targetNum[0] <= 0 && targetNum[1] <= 0 && targetNum[2] <= 0 && targetNum[3] <= 0)
        {
            gameClear.SetActive(true);
            Invoke("Result", 3.0f);
        }
        else if (TurnCS.nowTurn >= 20)
        {
            gameOver.SetActive(true);
            Invoke("Result", 3.0f);
        }
    }
}
