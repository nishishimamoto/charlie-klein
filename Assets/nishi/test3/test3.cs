﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class test3 : MonoBehaviour
{
    [SerializeField] CursorSelect cursorSelectCS;
    [SerializeField] TurnStart TurnCS;
    [SerializeField] test3timer TimerCS;
    [SerializeField] Combo ComboCS;
    [SerializeField] Explosion ExplosionCS;
    [SerializeField] Pause PauseCS;
    PanelAnim[] panelAnim = new PanelAnim[sidePanel];
    TurnOver[] TurnOverCS = new TurnOver[sidePanel];
    [SerializeField] Thinking ThinkingCS;

    const int mainPanel = 30;    //メインパネルの数
    const int sidePanel = 42;    //サイドパネルの数
    const int width = 7;   //横の数
    const int height = 6;    //縦の数
    const int colorNum = 3; //出現する色の数
    const float maxThinkingTime = 10;   //ゲーム開始前の考える時間

    int[] mainNumber = new int[mainPanel];     //3*3のナンバー
    int[] sideNumber = new int[sidePanel];     //4*4のナンバー
    int tmpNumber;          //数字入れ替え時の一時保存
    int[] bonusLevel = new int[sidePanel]; //サイドパネルのボーナス確認 (0=なし,1=あり)
    bool[] bonusFlg = new bool[sidePanel]; //このターン既にボーナスパネルになったかどうか
    int tmpBonus;         //ボーナス入れ替え時の一時保存
    int judgNum = 0;  //和を計算する配列
    public static int score;      //スコア

    int chooseMain = -1; //現在選んでいるメインナンバー

    bool isHorizontal;     //十字キーの左右入力がニュートラルにもどったか
    bool isVertical;    //十字キーの上下入力がニュートラルにもどったか

    //[SerializeField] Image[] sideImage; //サイドスフィアをいれる
    GameObject[] sideSphere = new GameObject[sidePanel]; //サイドスフィアをいれる
    GameObject[] turnOver = new GameObject[sidePanel];  //衛星の寿命を得る
    TurnOver tmpTurnOver;
    Color[] sideSphereColor = new Color[sidePanel];  //マテリアル色を変えるための仮入れ
    Color tmpSideColor;
    GameObject tmpObj;
    Renderer[] sideSphereRenderer = new Renderer[sidePanel];    //実際にオブジェクトの色を変更する
    //[SerializeField] GameObject[] obj;  //アニメーションさせるためのオブジェクト一時消し
    PanelAnim tmpAnim;

    //GameObject tmpObj;
    //[SerializeField] Image[] sideBonusFrame;一時消し
    GameObject[] mainSphere = new GameObject[mainPanel];
    Color[] mainSphereColor = new Color[mainPanel];  //マテリアル色を変えるための仮入れ
    Color tmpMainColor;
    Renderer[] mainSphereRenderer = new Renderer[mainPanel];    //実際にオブジェクトの色を変更する

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

    float alpha_Time = 0f;   //点滅させる時間
    float alpha_Sin;    //消すときに点滅させる
    bool alpha_Flg;
    int check = 0; //中身を順にみる変数

    bool[] flgCheck = new bool[mainPanel + 1];  //ポイントになった箇所を記憶,5はnull
    int mainColorNum = 0;               //全パネルが同じ色になったら色を変える
    int checkColorNum;

    bool[] panelMove = new bool[2]; //右か左にパネル移動させるフラグ
                                    //float changeTime = 0.1f;

    public Material[] _material;           // 割り当てるマテリアル.
    public Texture NormalmapTexture;

    [SerializeField] GameObject thinkingObjects;
    [SerializeField] Text thinkingTimeText;
    [SerializeField] GameObject gameOverVeil;
    [SerializeField] GameObject gameOverText;

    [SerializeField] GameObject main;
    [SerializeField] GameObject side;
    [SerializeField] bool[] isPlanet = new bool[mainPanel];

    public static string oldSceneName;  //リザルトから戻る用
    bool gameFinish;    //ゲームが終わったかどうかの判定

    float lightTime = 8f;
    float alphaDerayTime = 0;
    bool isAlphaLast;

    // Start is called before the first frame update
    void Start()
    {
        ThinkingCS.thinkingTime = maxThinkingTime;
        gameOverVeil.SetActive(false);
        gameOverText.SetActive(false);

        for (int i = 0; i < mainPanel; i++)
        {
            if (!isPlanet[i])
            {
                if (chooseMain < 0) chooseMain = i;
            }
        }

        SatelliteCreat();   //衛星生成

        scoreText = Score.GetComponent<Text>();

        ColorChange();   //パネルの色変更
        //TurnCS.nowTurn = 1; //ターン数の指定
        TimerCS.maxTime = 20.0f;
        TimerCS.timeCount = TimerCS.maxTime;
        score = 0;  //スコアの初期化
    }

    // Update is called once per frame
    void Update()
    {
        if (!PauseCS.isPause)
        {
            if (!gameFinish)
            {
                if (!alpha_Flg)
                {
                    if (!TurnCS.isTurnStart)
                    {
                        LifeLimmit();   //寿命0の衛星があったらリザルトに飛ぶ
                        TurnCS.GameStart();
                        SphereCreate(); //消した衛星の表示
                    }
                    else if (TurnCS.isTurnStart)
                    {
                        if (!TimerCS.countStart)
                        {
                            TimeReSet();    //スコアに応じてタイムのリセット
                            ThinkingCS.ThinkingTime(); //n秒で強制的にスタートさせる
                            if (ThinkingCS.thinkingTime <= 0)
                            {
                                TimerCS.countStart = true;
                                thinkingObjects.SetActive(false);
                            }
                        }
                        if (!panelMove[0] && !panelMove[1]) PanelOperation();   //パネルの操作
                        else if (panelMove[0] || panelMove[1]) PanelMove();        //パネルのアニメーション
                                                                                   //PointCheck();             //盤面が揃ったか見る 揃ったらすぐ変わる
                        PointBlinking();            //4つ揃ったときの点滅
                                                    //ColorChange();              //パネルの色変更
                        TimerCS.TimerCount();       //制限時間のカウントと表示
                        TurnEnd();                  //ターン終了時の処理

                        //SelectImageMove();  //現在選んでいるパネルの可視化 ここで呼ぶ
                        cursorSelectCS.SelectImageMove(chooseMain);
                    }
                }
                else if (alpha_Flg) alpha();
            }else if(gameFinish) Invoke("Result", 2);
        }
    }

    //***
    void alpha()
    {
        if ((flgCheck[check] && check <= (mainPanel - 1)) && !isPlanet[check])   //0~9で条件を満たしたら
        {

            if (alpha_Time < 0.6f) //0.6まで光る
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
            else if (alpha_Time >= 0.6f)//0.6以上になったらalpha_Timeを0に戻して爆破して次のをみる
            {
                //ComboCS.comboCount += 1;
                alpha_Time = 0;
                //flgCheck[check] = false;
                //ColorChange();

                //mainNumber[i] = mainColorNumber[0];

                if (addOrLoss[check] == 1)
                {
                    ScoreAdd();
                    ////ランダムな数値にいれかえ
                    //MainGenerate();

                    addOrLoss[check] = 9;
                }
                if (addOrLoss[check] == 0)
                {
                    ScoreLoss();
                    addOrLoss[check] = 9;
                }
                if (ComboCS.comboCount > 0) ComboCS.BoardCombo(check); //爆破箇所にコンボのパネル
                check += 1;
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
                        sideSphere[(check / (width - 1)) + check].SetActive(true);
                        sideSphere[(check / (width - 1)) + check + 1].SetActive(true);
                        sideSphere[(check / (width - 1)) + check + width].SetActive(true);
                        sideSphere[(check / (width - 1)) + check + width + 1].SetActive(true);
                    }
                }
            }
            else
            {
                alphaDerayTime += Time.deltaTime;
                if (!isAlphaLast)
                {
                    AlphaFlgDeray();
                    isAlphaLast = true;

                    for (int i = 0; i < mainPanel; i++)
                    {
                        if (flgCheck[i])
                        {
                            sideNumber[(i / (width - 1)) + i] = sideColorNumber[Random.Range(0, colorNum)];
                            sideNumber[(i / (width - 1)) + i + 1] = sideColorNumber[Random.Range(0, colorNum)];
                            sideNumber[(i / (width - 1)) + i + width + 1] = sideColorNumber[Random.Range(0, colorNum)];
                            sideNumber[(i / (width - 1)) + i + width] = sideColorNumber[Random.Range(0, colorNum)];

                            TurnOverCS[(i / (width - 1)) + i].LifeCountReSet();
                            TurnOverCS[(i / (width - 1)) + i + 1].LifeCountReSet();
                            TurnOverCS[(i / (width - 1)) + i + width + 1].LifeCountReSet();
                            TurnOverCS[(i / (width - 1)) + i + width].LifeCountReSet();

                            ////ランダムな数値にいれかえ
                            //MainGenerate();

                            ExplosionCS.particle[i].Play(); //条件を満たした惑星が爆発
                            if(ComboCS.comboCount < 3) ExplosionCS.audio.PlayOneShot(ExplosionCS.clip);//爆発のSEを再生
                            else if(ComboCS.comboCount < 6) ExplosionCS.audio.PlayOneShot(ExplosionCS.clip);//爆発のSEを再生

                                                                                      //Invoke("ExplosionStop", 1.0f);    //時間差で爆発を止める
                            ColorChange();   //パネルの色変更

                            sideSphere[(i / (width - 1)) + i].SetActive(false);
                            sideSphere[(i / (width - 1)) + i + 1].SetActive(false);
                            sideSphere[(i / (width - 1)) + i + width].SetActive(false);
                            sideSphere[(i / (width - 1)) + i + width + 1].SetActive(false);
                        }
                    }
                }
                else if (alphaDerayTime >= 2)
                {

                    alphaDerayTime = 0;
                    check = 0;
                    thinkingObjects.SetActive(true);
                    isAlphaLast = false;
                    alpha_Flg = false;
                }
            }
        }
        else check += 1;
    }
    //***
    void PanelOperation()
    {
        //パネル反時計回り
        if (Input.GetButtonDown("LB"))
        {
            panelMove[0] = true;
            if (!TimerCS.countStart) ThinkingCS.thinkingTime = 0;
        }
        //パネル時計回り
        else if (Input.GetButtonDown("RB"))
        {
            panelMove[1] = true;
            if (!TimerCS.countStart) ThinkingCS.thinkingTime = 0;
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
                                //isAddScore = true;
                                //ScoreAdd();
                            }
                            else
                            {
                                flgCheck[i] = true;
                                alpha_Flg = true;
                                lossScoreCount += 1;
                                addOrLoss[i] = 0;
                                //isLossScore = false;
                                //ScoreLoss();
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
        //for (int i = 0; i < mainPanel; i++)
        //{
        //    switch (mainNumber[i])
        //    {
        //        case 4:
        //            //mainSphereColor[i] = Color.red;
        //            //mainSphere[i].GetComponent<Renderer>().material.color = mainSphereColor[i];
        //            //mainSphere[i].GetComponent<Renderer>().material = _material[3];
        //            break;
        //        case 32:
        //            //mainSphereColor[i] = Color.blue;
        //            //mainSphere[i].GetComponent<Renderer>().material.color = mainSphereColor[i];
        //            //mainSphere[i].GetComponent<Renderer>().material = _material[4];
        //            break;
        //        case 128:
        //            //mainSphereColor[i] = Color.yellow;
        //            //mainSphere[i].GetComponent<Renderer>().material.color = mainSphereColor[i];
        //            break;
        //        case 512:
        //            //mainSphereColor[i] = Color.green;
        //            //mainSphere[i].GetComponent<Renderer>().material.color = mainSphereColor[i];
        //            break;
        //        default:
        //            break;
        //    }
        //}
        switch (mainNumber[0])
        {
            case 4:
                //mainSphereColor[i] = Color.red;
                //mainSphere[i].GetComponent<Renderer>().material.color = mainSphereColor[i];
                //mainSphere[i].GetComponent<Renderer>().material = _material[3];
                mainSphere[0].GetComponent<Renderer>().material = _material[0];
                break;
            case 32:
                //mainSphereColor[i] = Color.blue;
                //mainSphere[i].GetComponent<Renderer>().material.color = mainSphereColor[i];
                //mainSphere[i].GetComponent<Renderer>().material = _material[4];
                mainSphere[0].GetComponent<Renderer>().material = _material[1];
                break;
            case 128:
                //mainSphereColor[i] = Color.yellow;
                //mainSphere[i].GetComponent<Renderer>().material.color = mainSphereColor[i];
                mainSphere[0].GetComponent<Renderer>().material = _material[2];
                break;
            case 512:
                //mainSphereColor[i] = Color.green;
                //mainSphere[i].GetComponent<Renderer>().material.color = mainSphereColor[i];
                mainSphere[0].GetComponent<Renderer>().material = _material[3];
                break;
            default:
                break;
        }

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

            //スクリプト入れ替え
            tmpTurnOver = TurnOverCS[(chooseMain / (width - 1)) + chooseMain];
            TurnOverCS[(chooseMain / (width - 1)) + chooseMain] = TurnOverCS[(chooseMain / (width - 1)) + chooseMain + 1];
            TurnOverCS[(chooseMain / (width - 1)) + chooseMain + 1] = TurnOverCS[(chooseMain / (width - 1)) + chooseMain + width + 1];
            TurnOverCS[(chooseMain / (width - 1)) + chooseMain + width + 1] = TurnOverCS[(chooseMain / (width - 1)) + chooseMain + width];
            TurnOverCS[(chooseMain / (width - 1)) + chooseMain + width] = tmpTurnOver;



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

            ////スクリプト入れ替え
            tmpTurnOver = TurnOverCS[(chooseMain / (width - 1)) + chooseMain];
            TurnOverCS[(chooseMain / (width - 1)) + chooseMain] = TurnOverCS[(chooseMain / (width - 1)) + chooseMain + width];
            TurnOverCS[(chooseMain / (width - 1)) + chooseMain + width] = TurnOverCS[(chooseMain / (width - 1)) + chooseMain + width + 1];
            TurnOverCS[(chooseMain / (width - 1)) + chooseMain + width + 1] = TurnOverCS[(chooseMain / (width - 1)) + chooseMain + 1];
            TurnOverCS[(chooseMain / (width - 1)) + chooseMain + 1] = tmpTurnOver;

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
        score += 100 + (50 * ComboCS.comboCount);

        scoreText.text = "" + score;

        ComboCS.comboTime = 5.0f;
        ComboCS.comboCount += 1;
        addScoreCount -= 1;
    }

    void ScoreLoss()
    {
        //スコア100と1コンボ50
        //score -= 100;
        score += 100 + (50 * ComboCS.comboCount);

        scoreText.text = "" + score;

        ComboCS.comboTime = 5.0f;
        ComboCS.comboCount += 1;
        lossScoreCount -= 1;
    }

    void TurnEnd()
    {

        if ((Input.GetButtonDown("A") || TimerCS.timeCount <= 0) && TimerCS.countStart == true) //Aか制限時間でターン終了
        {
            //時間が１になってしまうので０にしてテキスト更新
            TimerCS.timeCount = 0;
            TimerCS.TimerCount();

            for (int f = 0; f < mainPanel; f++) flgCheck[f] = false; //念のため別のforでfalseにする
            if (!alpha_Flg) PointCheck();
            TimerCS.timeOut = false;
            TimerCS.countStart = false;
            TimerCS.bigTimerText.enabled = false;
            ComboCS.comboCount = 0; //コンボカウントリセット
            LifeDown(); //衛星の寿命を縮める
            ThinkingCS.thinkingTime = maxThinkingTime;
            //thinkingObjects.SetActive(true);
            TurnCS.turn += 1;
            TurnCS.isTurnStart = false;
            lightTime = 8;

            //TurnCS.TurnCount();        //経過ターンの更新表示
        }
        //else
        //{
        //    if (TimerCS.timeCount > 0) TimerCS.timeCount = 0f;
        //    //リザルト画面へ
        //    oldSceneName = SceneManager.GetActiveScene().name;
        //    SceneManager.LoadScene("Result");
        //}

        //if (TimerCS.timeCount <= 0f) //Xか制限時間でターン終了
        //{
        //    if (TimerCS.timeCount > 0) TimerCS.timeCount = 0f;
        //    //リザルト画面へ
        //    oldSceneName = SceneManager.GetActiveScene().name;
        //    SceneManager.LoadScene("testresurt");
        //}

    }
    void PointBlinking()
    {
        //for (int i = 0; i < mainPanel; i++)
        //{
        //    if (!isPlanet[i])
        //    {

        //        judgNum = mainNumber[i];

        //        if (judgNum == sideNumber[(i / (width - 1)) + i] * 4 || sideNumber[(i / (width - 1)) + i] == 32)
        //            if (judgNum == sideNumber[(i / (width - 1)) + i + 1] * 4 || sideNumber[(i / (width - 1)) + i + 1] == 32)
        //                if (judgNum == sideNumber[(i / (width - 1)) + i + width + 1] * 4 || sideNumber[(i / (width - 1)) + i + width + 1] == 32)
        //                    if (judgNum == sideNumber[(i / (width - 1)) + i + width] * 4 || sideNumber[(i / (width - 1)) + i + width] == 32) //色を満たした
        //                    {
        //                        float blinking = 0f;
        //                        float blinkingSpeed = 2.0f;
        //                        blinking = Mathf.Sin(2 * Mathf.PI * blinkingSpeed * Time.time); //sin波取得 点滅(-1~1)

        //                        //mainSphere[i].GetComponent<Renderer>().material.SetFloat("_AtmosphereDensity", 6 + blinking);草
        //                        sideSphere[(i / (width - 1)) + i].GetComponent<Renderer>().material.SetFloat("_AtmosphereDensity", 6 + blinking);
        //                        sideSphere[(i / (width - 1)) + i + 1].GetComponent<Renderer>().material.SetFloat("_AtmosphereDensity", 6 + blinking);
        //                        sideSphere[(i / (width - 1)) + i + width].GetComponent<Renderer>().material.SetFloat("_AtmosphereDensity", 6 + blinking);
        //                        sideSphere[(i / (width - 1)) + i + width + 1].GetComponent<Renderer>().material.SetFloat("_AtmosphereDensity", 6 + blinking);
        //                        //Debug.Log(blinking);
        //                    }
        //        judgNum = 0;
        //    }
        //}

        for (int i = 0; i < mainPanel; i++)
        {
            if (!isPlanet[i])
            {
                judgNum = mainNumber[0];

                if (sideNumber[(i / (width - 1)) + i] * 4 == sideNumber[(i / (width - 1)) + i + 1] * 4)
                    if (sideNumber[(i / (width - 1)) + i + 1] * 4 == sideNumber[(i / (width - 1)) + i + width + 1] * 4)
                        if (sideNumber[(i / (width - 1)) + i + width + 1] * 4 == sideNumber[(i / (width - 1)) + i + width] * 4)
                        {
                            if (mainNumber[0] == sideNumber[(i / (width - 1)) + i + width] * 4) //色を満たした
                            {
                                float blinking = 0f;
                                float blinkingSpeed = 2.0f;
                                blinking = Mathf.Sin(2 * Mathf.PI * blinkingSpeed * Time.time); //sin波取得 点滅(-1~1)

                                sideSphere[(i / (width - 1)) + i].GetComponent<Renderer>().material.SetFloat("_AtmosphereDensity", 6 + blinking);
                                sideSphere[(i / (width - 1)) + i + 1].GetComponent<Renderer>().material.SetFloat("_AtmosphereDensity", 6 + blinking);
                                sideSphere[(i / (width - 1)) + i + width].GetComponent<Renderer>().material.SetFloat("_AtmosphereDensity", 6 + blinking);
                                sideSphere[(i / (width - 1)) + i + width + 1].GetComponent<Renderer>().material.SetFloat("_AtmosphereDensity", 6 + blinking);
                            }
                            else
                            {
                                float blinking = 0f;
                                float blinkingSpeed = 2.0f;
                                blinking = Mathf.Sin(2 * Mathf.PI * blinkingSpeed * Time.time); //sin波取得 点滅(-1~1)

                                sideSphere[(i / (width - 1)) + i].GetComponent<Renderer>().material.SetFloat("_AtmosphereDensity", 6 + blinking);
                                sideSphere[(i / (width - 1)) + i + 1].GetComponent<Renderer>().material.SetFloat("_AtmosphereDensity", 6 + blinking);
                                sideSphere[(i / (width - 1)) + i + width].GetComponent<Renderer>().material.SetFloat("_AtmosphereDensity", 6 + blinking);
                                sideSphere[(i / (width - 1)) + i + width + 1].GetComponent<Renderer>().material.SetFloat("_AtmosphereDensity", 6 + blinking);
                            }
                        }

                judgNum = 0;
            }
        }
    }

    void ExplosionStop()
    {
        for (int i = 0; i < mainPanel; i++)
        {
            ExplosionCS.particle[i].Stop();
        }
    }

    void MainGenerate()
    {

        do
        {
            //ランダムな数値にいれかえ
            mainNumber[0] = mainColorNumber[Random.Range(0, colorNum)];
            checkColorNum = 0;

            for (int i = 0; i < sidePanel; i++)
            {

                if (sideNumber[i] * 4 == mainNumber[0])
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
        SceneManager.LoadScene("testresurt");
    }

    void TimeLimmit()
    {
        if (TimerCS.maxTime > 10) TimerCS.maxTime = 20 - (score / 3000);
        //if (TimerCS.maxTime > 10) TimerCS.maxTime = 60;
        else if (TimerCS.maxTime <= 10) TimerCS.maxTime = 10;
    }

    void TimeReSet()
    {
        //スコアに応じて時間の設定
        TimeLimmit();
        TimerCS.timeCount = TimerCS.maxTime;
    }

    void LifeLimmit()
    {
        for (int i = 0; i < sidePanel; i++)
        {
            if (TurnOverCS[i] != null && TurnOverCS[i].lifeSpan == 0 && TurnCS.turn > 2)
            {
                sideSphere[i].transform.Translate(0f, 0f, -1);
                if (!gameOverVeil.activeSelf) gameOverVeil.SetActive(true);
                if(!gameOverText.activeSelf) gameOverText.SetActive(true);
                //Invoke("Result", 0.5f);
                gameFinish = true;
            }
        }
    }

    void LifeDown()
    {
        for (int i = 0; i < sidePanel; i++)
        {
            if (TurnOverCS[i] != null)
            {
                TurnOverCS[i].LifeCountDown();
            }
        }
    }

    void SphereCreate()
    {
        if (lightTime >= 6.0f) lightTime -= Time.deltaTime * 2f;
        for (int i = 0; i < mainPanel; i++)
        {
            if (flgCheck[i])
            {
                sideSphere[(i / (width - 1)) + i].SetActive(true);
                sideSphere[(i / (width - 1)) + i + 1].SetActive(true);
                sideSphere[(i / (width - 1)) + i + width].SetActive(true);
                sideSphere[(i / (width - 1)) + i + width + 1].SetActive(true);

                sideSphere[(i / (width - 1)) + i].GetComponent<Renderer>().material.SetFloat("_AtmosphereDensity", lightTime);
                sideSphere[(i / (width - 1)) + i + 1].GetComponent<Renderer>().material.SetFloat("_AtmosphereDensity", lightTime);
                sideSphere[(i / (width - 1)) + i + width].GetComponent<Renderer>().material.SetFloat("_AtmosphereDensity", lightTime);
                sideSphere[(i / (width - 1)) + i + width + 1].GetComponent<Renderer>().material.SetFloat("_AtmosphereDensity", lightTime);

                //flgCheck[i] = false;
            }
        }
    }

    void SatelliteCreat()
    {
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
                    //各衛星の子オブジェクトのTextを得る
                    turnOver[i] = sideSphere[i].transform.Find("TurnOver").gameObject;
                    TurnOverCS[i] = turnOver[i].GetComponent<TurnOver>();

                    sideNumber[i] = sideColorNumber[Random.Range(0, colorNum)];
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
                    //各衛星の子オブジェクトのTextを得る
                    turnOver[i] = sideSphere[i].transform.Find("TurnOver").gameObject;
                    TurnOverCS[i] = turnOver[i].GetComponent<TurnOver>();
                    sideNumber[i] = sideColorNumber[Random.Range(0, colorNum)];
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
                    //各衛星の子オブジェクトのTextを得る
                    turnOver[i] = sideSphere[i].transform.Find("TurnOver").gameObject;
                    TurnOverCS[i] = turnOver[i].GetComponent<TurnOver>();
                    sideNumber[i] = sideColorNumber[Random.Range(0, colorNum)];
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
                    //各衛星の子オブジェクトのTextを得る
                    turnOver[i] = sideSphere[i].transform.Find("TurnOver").gameObject;
                    TurnOverCS[i] = turnOver[i].GetComponent<TurnOver>();
                    sideNumber[i] = sideColorNumber[Random.Range(0, colorNum)];
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
                    //各衛星の子オブジェクトのTextを得る
                    turnOver[i] = sideSphere[i].transform.Find("TurnOver").gameObject;
                    TurnOverCS[i] = turnOver[i].GetComponent<TurnOver>();
                    sideNumber[i] = sideColorNumber[Random.Range(0, colorNum)];
                }
            }
        }
    }

    void AlphaFlgDeray()
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
                    //for (int f = 0; f < mainPanel; f++) flgCheck[f] = false; //念のため別のforでfalseにする
        mainColorNum = 0;
        ColorChange();   //パネルの色変更
        rainbow = false;
        rainbowTarget = 0;
    }
}
