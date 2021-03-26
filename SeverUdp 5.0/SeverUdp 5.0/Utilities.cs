using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeverUdp_5._0
{
    public class Utilities
    {
        public struct Sys1
        {

            public byte[] pref; // признак начала файла, при разметке проинициализировано "vibr"
            public float K1_skz_yel; // коэф-т для допуска СКЗ диапазон1 граница желтого выхода за норму
            public float K2_skz_yel; // коэф-т для допуска СКЗ диапазон2 граница желтого выхода за норму
            public float K1_pik_yel; // коэф-т для допуска Пик-фактора диапазон1 граница желтого выхода за норму
            public float K2_pik_yel; // коэф-т для допуска Пик-фактора диапазон2 граница желтого выхода за норму
            public float K1_skz_red; // коэф-т для допуска СКЗ диапазон1 граница красного выхода за норму
            public float K2_skz_red; // коэф-т для допуска СКЗ диапазон2 граница красного выхода за норму
            public float n_zub; // количество зубьев для вычисления F_ob_min
            public float Kd_rmk1; // коэф-т для нормализации значений вибродатчика 1 (перевод СКЗ в м/с**2)
            public float Kd_rmk2; // коэф-ты для нормализации значений вибродатчика 2 (перевод СКЗ в м/с**2)
            public float K_Moment_rmk1; // граница желтого выхода за норму вибродатчиков 1 и 2
            public float K_Moment_rmk2; // граница красного выхода за норму вибродатчиков 1 и 2
            public UInt16 graniza_1; // нижняя граница для определения диапазонов по об/мин
            public UInt16 graniza_2; // верхняя граница для определения диапазонов по об/мин
            public UInt16 moment_1; // нижняя граница для определения диапазонов по нагрузке
            public UInt16 moment_2; // верхняя граница для определения диапазонов по нагрузке
            public UInt16 kratnoct_FOB; //кратность записи ФОВ( 1..100 )
            public UInt32 period_recording; // периодичность записи в секундах (сейчас в F_sys2)
            public UInt16 regim_work; // режим работы ( пока не используется)

        }


        public struct COUNT_F
        {

            public byte O1; // счетчик образцового файла массива диапазона1
            public byte O2; // счетчик образцового файла массива диапазона2
            public UInt16 T1; // счетчик текущего файла массива диапазона1
            public UInt16 T2; // счетчик текущего файла массива диапазона2
            public byte O31; // счетчик элементов образцового файла ФДП диапазона1
            public byte O32; // счетчик элементов образцового файла ФДП диапазона2
            public UInt16 T31; // счетчик элементов текущего файла ФДП диапазона1
            public UInt16 T32; // счетчик элементов текущего файла ФДП диапазона2

        }//12
        public struct sys2
        {

            public COUNT_F count; // счетчики файлов 12 byte

            public float Sr_skz_d1, Sr_skz_d2, Sr_pik_d1, Sr_pik_d2; // средние значения ФДП

            public float pred_skz_d1, pred_skz_d2, pred_pik_d1, pred_pik_d2; // предыдущие значения ФДП

            public float Sr_M_d1, Sr_M_d2; // средние значения ФДП --мощность

            public UInt32 Cnt_Bad_red, Cnt_Bad_yel; // счетчики "хорошо"- "плохо" для индикации лампочкой

            public float Sum_skz_d1, Sum_pik_d1, Sum_skz_d2, Sum_pik_d2; // суммы для вычисления средних скз, пик

            public float Prozent_yel, Prozent_red; // процент уровня состояния желтый, красный

            public UInt16 cnt_d1, cnt_d2; // счетчики для вычисления средних

            public UInt16 defect_file; // кол-во бракованных файлов

        }//85


        public struct COUNT_F_ALL // структура счетчиков всех файлов для сервера, используется в f_Sys2

        {
            public UInt32 O1; // счетчик образцового файла массива диапазона1

            public UInt32 O2; // счетчик образцового файла массива диапазона2

            public UInt32 T1; // счетчик текущего файла массива диапазона1

            public UInt32 T2; // счетчик текущего файла массива диапазона2

            public UInt32 O31; // счетчик элементов образцового файла ФДП диапазона1

            public UInt32 O32; // счетчик элементов образцового файла ФДП диапазона2

            public UInt32 T31; // счетчик элементов текущего файла ФДП диапазона1

            public UInt32 T32; // счетчик элементов текущего файла ФДП диапазона2
        }//32
        public struct F_sys2
        {
            public UInt32 cnt; // абсолютный счетчик записи, изменяется функцией записи
                               // при чтении f_Sys2 если cnt=0 то это первое чтение после разметки 

            public sys2[] rmk;
            public UInt16 nomer_rmk; // номер РМК, с которым работаем // 0 -- rmk1, 1 -- rmk2
            public UInt16 regim_work_tek; // периодичность записи в секундах 
            public COUNT_F_ALL[] count_ALL;
        }
        public void Sys1_(byte[] byteArray, byte[] byteArrayAfterStruct, string path)
        {
            Sys1 sys1;
            sys1.pref = new byte[8];

            //заносим полученные данные в структуру sys1
            Array.Copy(byteArray, 8, sys1.pref, 0, 8);//начинаем копировать с 8 элемента (второй параметр), так как первые 8 байт - это заголовок.
            sys1.K1_skz_yel = BitConverter.ToSingle(byteArray, 16);
            sys1.K2_skz_yel = BitConverter.ToSingle(byteArray, 20);
            sys1.K1_pik_yel = BitConverter.ToSingle(byteArray, 24);
            sys1.K2_pik_yel = BitConverter.ToSingle(byteArray, 28);
            sys1.K1_skz_red = BitConverter.ToSingle(byteArray, 32);
            sys1.K2_skz_red = BitConverter.ToSingle(byteArray, 36);
            sys1.n_zub = BitConverter.ToSingle(byteArray, 40);
            sys1.Kd_rmk1 = BitConverter.ToSingle(byteArray, 44);
            sys1.Kd_rmk2 = BitConverter.ToSingle(byteArray, 48);
            sys1.K_Moment_rmk1 = BitConverter.ToSingle(byteArray, 52);
            sys1.K_Moment_rmk2 = BitConverter.ToSingle(byteArray, 56);
            sys1.graniza_1 = BitConverter.ToUInt16(byteArray, 60);
            sys1.graniza_2 = BitConverter.ToUInt16(byteArray, 62);
            sys1.moment_1 = BitConverter.ToUInt16(byteArray, 64);
            sys1.moment_2 = BitConverter.ToUInt16(byteArray, 66);
            sys1.kratnoct_FOB = BitConverter.ToUInt16(byteArray, 68);
            sys1.period_recording = BitConverter.ToUInt32(byteArray, 70);
            sys1.regim_work = BitConverter.ToUInt16(byteArray, 74);

            //достаём данные с структуры Sys1
            Array.Copy(sys1.pref, 0, byteArrayAfterStruct, 0, 8);
            BitConverter.GetBytes(sys1.K1_skz_yel).CopyTo(byteArrayAfterStruct, 8);
            BitConverter.GetBytes(sys1.K2_skz_yel).CopyTo(byteArrayAfterStruct, 12);
            BitConverter.GetBytes(sys1.K1_pik_yel).CopyTo(byteArrayAfterStruct, 16);
            BitConverter.GetBytes(sys1.K2_pik_yel).CopyTo(byteArrayAfterStruct, 20);
            BitConverter.GetBytes(sys1.K1_skz_red).CopyTo(byteArrayAfterStruct, 24);
            BitConverter.GetBytes(sys1.K2_skz_red).CopyTo(byteArrayAfterStruct, 28);
            BitConverter.GetBytes(sys1.n_zub).CopyTo(byteArrayAfterStruct, 32);
            BitConverter.GetBytes(sys1.Kd_rmk1).CopyTo(byteArrayAfterStruct, 36);
            BitConverter.GetBytes(sys1.Kd_rmk2).CopyTo(byteArrayAfterStruct, 40);
            BitConverter.GetBytes(sys1.K_Moment_rmk1).CopyTo(byteArrayAfterStruct, 44);
            BitConverter.GetBytes(sys1.K_Moment_rmk2).CopyTo(byteArrayAfterStruct, 48);

            BitConverter.GetBytes(sys1.graniza_1).CopyTo(byteArrayAfterStruct, 52);
            BitConverter.GetBytes(sys1.graniza_2).CopyTo(byteArrayAfterStruct, 54);
            BitConverter.GetBytes(sys1.moment_1).CopyTo(byteArrayAfterStruct, 56);
            BitConverter.GetBytes(sys1.moment_2).CopyTo(byteArrayAfterStruct, 58);
            BitConverter.GetBytes(sys1.kratnoct_FOB).CopyTo(byteArrayAfterStruct, 60);
            BitConverter.GetBytes(sys1.period_recording).CopyTo(byteArrayAfterStruct, 62);
            BitConverter.GetBytes(sys1.regim_work).CopyTo(byteArrayAfterStruct, 66);

            using (var stream = new System.IO.FileStream(path, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write))//запись в файл
                                                                                                                            //FileMode.OpenOrCreate- ОС открывает файл,если он существует,
                                                                                                                            //в противном случае создаёт файл
            {
                //Перемещаемся в файле на 100 байт от начала
                //stream.Seek(0, System.IO.SeekOrigin.Begin);
                //Записываем буфер
                stream.Write(byteArrayAfterStruct, 0, byteArrayAfterStruct.Length - 8);
            }
        }

        public void Sys2_(byte[] byteArray, byte[] byteArrayAfterStruct, string path)
        {
            F_sys2 f_sys2new;
            f_sys2new.rmk = new sys2[2];
            f_sys2new.count_ALL = new COUNT_F_ALL[2];

            //заносим полученные данные в структуру sys2
            f_sys2new.cnt = BitConverter.ToUInt32(byteArray, 8);
            f_sys2new.rmk[0].count.O1 = byteArray[12];
            f_sys2new.rmk[0].count.O2 = byteArray[13];
            f_sys2new.rmk[0].count.T1 = BitConverter.ToUInt16(byteArray, 14);
            f_sys2new.rmk[0].count.T2 = BitConverter.ToUInt16(byteArray, 16);
            f_sys2new.rmk[0].count.O31 = byteArray[18];
            f_sys2new.rmk[0].count.O32 = byteArray[19];
            f_sys2new.rmk[0].count.T31 = BitConverter.ToUInt16(byteArray, 20);
            f_sys2new.rmk[0].count.T32 = BitConverter.ToUInt16(byteArray, 22);

            f_sys2new.rmk[0].Sr_skz_d1 = BitConverter.ToSingle(byteArray, 24);
            f_sys2new.rmk[0].Sr_skz_d2 = BitConverter.ToSingle(byteArray, 28);
            f_sys2new.rmk[0].Sr_pik_d1 = BitConverter.ToSingle(byteArray, 32);
            f_sys2new.rmk[0].Sr_pik_d2 = BitConverter.ToSingle(byteArray, 36);

            f_sys2new.rmk[0].pred_skz_d1 = BitConverter.ToSingle(byteArray, 40);
            f_sys2new.rmk[0].pred_skz_d2 = BitConverter.ToSingle(byteArray, 44);
            f_sys2new.rmk[0].pred_pik_d1 = BitConverter.ToSingle(byteArray, 48);
            f_sys2new.rmk[0].pred_pik_d2 = BitConverter.ToSingle(byteArray, 52);

            f_sys2new.rmk[0].Sr_M_d1 = BitConverter.ToSingle(byteArray, 56);
            f_sys2new.rmk[0].Sr_M_d2 = BitConverter.ToSingle(byteArray, 60);

            f_sys2new.rmk[0].Cnt_Bad_red = BitConverter.ToUInt32(byteArray, 64);
            f_sys2new.rmk[0].Cnt_Bad_yel = BitConverter.ToUInt32(byteArray, 68);

            f_sys2new.rmk[0].Sum_skz_d1 = BitConverter.ToSingle(byteArray, 72);
            f_sys2new.rmk[0].Sum_pik_d1 = BitConverter.ToSingle(byteArray, 76);
            f_sys2new.rmk[0].Sum_skz_d2 = BitConverter.ToSingle(byteArray, 80);
            f_sys2new.rmk[0].Sum_pik_d2 = BitConverter.ToSingle(byteArray, 84);

            f_sys2new.rmk[0].Prozent_yel = BitConverter.ToSingle(byteArray, 88);
            f_sys2new.rmk[0].Prozent_red = BitConverter.ToSingle(byteArray, 92);

            f_sys2new.rmk[0].cnt_d1 = BitConverter.ToUInt16(byteArray, 96);
            f_sys2new.rmk[0].cnt_d2 = BitConverter.ToUInt16(byteArray, 98);

            f_sys2new.rmk[0].defect_file = BitConverter.ToUInt16(byteArray, 100);

            f_sys2new.rmk[1].count.O1 = byteArray[102];
            f_sys2new.rmk[1].count.O2 = byteArray[103];
            f_sys2new.rmk[1].count.T1 = BitConverter.ToUInt16(byteArray, 104);
            f_sys2new.rmk[1].count.T2 = BitConverter.ToUInt16(byteArray, 106);
            f_sys2new.rmk[1].count.O31 = byteArray[108];
            f_sys2new.rmk[1].count.O32 = byteArray[109];
            f_sys2new.rmk[1].count.T31 = BitConverter.ToUInt16(byteArray, 110);
            f_sys2new.rmk[1].count.T32 = BitConverter.ToUInt16(byteArray, 112);

            f_sys2new.rmk[1].Sr_skz_d1 = BitConverter.ToSingle(byteArray, 114);
            f_sys2new.rmk[1].Sr_skz_d2 = BitConverter.ToSingle(byteArray, 118);
            f_sys2new.rmk[1].Sr_pik_d1 = BitConverter.ToSingle(byteArray, 122);
            f_sys2new.rmk[1].Sr_pik_d2 = BitConverter.ToSingle(byteArray, 126);

            f_sys2new.rmk[1].pred_skz_d1 = BitConverter.ToSingle(byteArray, 130);
            f_sys2new.rmk[1].pred_skz_d2 = BitConverter.ToSingle(byteArray, 134);
            f_sys2new.rmk[1].pred_pik_d1 = BitConverter.ToSingle(byteArray, 138);
            f_sys2new.rmk[1].pred_pik_d2 = BitConverter.ToSingle(byteArray, 142);

            f_sys2new.rmk[1].Sr_M_d1 = BitConverter.ToSingle(byteArray, 146);
            f_sys2new.rmk[1].Sr_M_d2 = BitConverter.ToSingle(byteArray, 150);

            f_sys2new.rmk[1].Cnt_Bad_red = BitConverter.ToUInt32(byteArray, 154);
            f_sys2new.rmk[1].Cnt_Bad_yel = BitConverter.ToUInt32(byteArray, 158);

            f_sys2new.rmk[1].Sum_skz_d1 = BitConverter.ToSingle(byteArray, 162);
            f_sys2new.rmk[1].Sum_pik_d1 = BitConverter.ToSingle(byteArray, 166);
            f_sys2new.rmk[1].Sum_skz_d2 = BitConverter.ToSingle(byteArray, 170);
            f_sys2new.rmk[1].Sum_pik_d2 = BitConverter.ToSingle(byteArray, 174);

            f_sys2new.rmk[1].Prozent_yel = BitConverter.ToSingle(byteArray, 178);
            f_sys2new.rmk[1].Prozent_red = BitConverter.ToSingle(byteArray, 182);

            f_sys2new.rmk[1].cnt_d1 = BitConverter.ToUInt16(byteArray, 186);
            f_sys2new.rmk[1].cnt_d2 = BitConverter.ToUInt16(byteArray, 188);

            f_sys2new.rmk[1].defect_file = BitConverter.ToUInt16(byteArray, 190);


            f_sys2new.nomer_rmk = BitConverter.ToUInt16(byteArray, 192);

            f_sys2new.regim_work_tek = BitConverter.ToUInt16(byteArray, 194);

            f_sys2new.count_ALL[0].O1 = BitConverter.ToUInt32(byteArray, 196);
            f_sys2new.count_ALL[0].O2 = BitConverter.ToUInt32(byteArray, 200);
            f_sys2new.count_ALL[0].T1 = BitConverter.ToUInt32(byteArray, 204);
            f_sys2new.count_ALL[0].T2 = BitConverter.ToUInt32(byteArray, 208);
            f_sys2new.count_ALL[0].O31 = BitConverter.ToUInt32(byteArray, 212);
            f_sys2new.count_ALL[0].O32 = BitConverter.ToUInt32(byteArray, 216);
            f_sys2new.count_ALL[0].T31 = BitConverter.ToUInt32(byteArray, 220);
            f_sys2new.count_ALL[0].T32 = BitConverter.ToUInt32(byteArray, 224);

            //2


            f_sys2new.count_ALL[1].O1 = BitConverter.ToUInt32(byteArray, 228);
            f_sys2new.count_ALL[1].O2 = BitConverter.ToUInt32(byteArray, 232);
            f_sys2new.count_ALL[1].T1 = BitConverter.ToUInt32(byteArray, 236);
            f_sys2new.count_ALL[1].T2 = BitConverter.ToUInt32(byteArray, 240);
            f_sys2new.count_ALL[1].O31 = BitConverter.ToUInt32(byteArray, 244);
            f_sys2new.count_ALL[1].O32 = BitConverter.ToUInt32(byteArray, 248);
            f_sys2new.count_ALL[1].T31 = BitConverter.ToUInt32(byteArray, 252);
            f_sys2new.count_ALL[1].T32 = BitConverter.ToUInt32(byteArray, 256);


            //достаём данные с структуры Sys2
            BitConverter.GetBytes(f_sys2new.cnt).CopyTo(byteArrayAfterStruct, 8);//8

            BitConverter.GetBytes(f_sys2new.rmk[0].count.O1).CopyTo(byteArrayAfterStruct, 12);
            BitConverter.GetBytes(f_sys2new.rmk[0].count.O2).CopyTo(byteArrayAfterStruct, 13);
            BitConverter.GetBytes(f_sys2new.rmk[0].count.T1).CopyTo(byteArrayAfterStruct, 14);
            BitConverter.GetBytes(f_sys2new.rmk[0].count.T2).CopyTo(byteArrayAfterStruct, 16);
            BitConverter.GetBytes(f_sys2new.rmk[0].count.O31).CopyTo(byteArrayAfterStruct, 18);
            BitConverter.GetBytes(f_sys2new.rmk[0].count.O32).CopyTo(byteArrayAfterStruct, 19);
            BitConverter.GetBytes(f_sys2new.rmk[0].count.T31).CopyTo(byteArrayAfterStruct, 20);
            BitConverter.GetBytes(f_sys2new.rmk[0].count.T32).CopyTo(byteArrayAfterStruct, 22);

            BitConverter.GetBytes(f_sys2new.rmk[0].Sr_skz_d1).CopyTo(byteArrayAfterStruct, 24);
            BitConverter.GetBytes(f_sys2new.rmk[0].Sr_skz_d2).CopyTo(byteArrayAfterStruct, 28);
            BitConverter.GetBytes(f_sys2new.rmk[0].Sr_pik_d1).CopyTo(byteArrayAfterStruct, 32);
            BitConverter.GetBytes(f_sys2new.rmk[0].Sr_pik_d2).CopyTo(byteArrayAfterStruct, 36);

            BitConverter.GetBytes(f_sys2new.rmk[0].pred_skz_d1).CopyTo(byteArrayAfterStruct, 40);
            BitConverter.GetBytes(f_sys2new.rmk[0].pred_skz_d2).CopyTo(byteArrayAfterStruct, 44);
            BitConverter.GetBytes(f_sys2new.rmk[0].pred_pik_d1).CopyTo(byteArrayAfterStruct, 48);
            BitConverter.GetBytes(f_sys2new.rmk[0].pred_pik_d2).CopyTo(byteArrayAfterStruct, 52);

            BitConverter.GetBytes(f_sys2new.rmk[0].Sr_M_d1).CopyTo(byteArrayAfterStruct, 56);
            BitConverter.GetBytes(f_sys2new.rmk[0].Sr_M_d2).CopyTo(byteArrayAfterStruct, 60);

            BitConverter.GetBytes(f_sys2new.rmk[0].Cnt_Bad_red).CopyTo(byteArrayAfterStruct, 64);
            BitConverter.GetBytes(f_sys2new.rmk[0].Cnt_Bad_yel).CopyTo(byteArrayAfterStruct, 68);

            BitConverter.GetBytes(f_sys2new.rmk[0].Sum_skz_d1).CopyTo(byteArrayAfterStruct, 72);
            BitConverter.GetBytes(f_sys2new.rmk[0].Sum_pik_d1).CopyTo(byteArrayAfterStruct, 76);
            BitConverter.GetBytes(f_sys2new.rmk[0].Sum_skz_d2).CopyTo(byteArrayAfterStruct, 80);
            BitConverter.GetBytes(f_sys2new.rmk[0].Sum_pik_d2).CopyTo(byteArrayAfterStruct, 84);

            BitConverter.GetBytes(f_sys2new.rmk[0].Prozent_yel).CopyTo(byteArrayAfterStruct, 88);
            BitConverter.GetBytes(f_sys2new.rmk[0].Prozent_red).CopyTo(byteArrayAfterStruct, 92);

            BitConverter.GetBytes(f_sys2new.rmk[0].cnt_d1).CopyTo(byteArrayAfterStruct, 94);
            BitConverter.GetBytes(f_sys2new.rmk[0].cnt_d2).CopyTo(byteArrayAfterStruct, 98);

            BitConverter.GetBytes(f_sys2new.rmk[0].defect_file).CopyTo(byteArrayAfterStruct, 100);

            BitConverter.GetBytes(f_sys2new.nomer_rmk).CopyTo(byteArrayAfterStruct, 102);

            BitConverter.GetBytes(f_sys2new.regim_work_tek).CopyTo(byteArrayAfterStruct, 104);

            BitConverter.GetBytes(f_sys2new.count_ALL[0].O1).CopyTo(byteArrayAfterStruct, 106);
            BitConverter.GetBytes(f_sys2new.count_ALL[0].O2).CopyTo(byteArrayAfterStruct, 110);
            BitConverter.GetBytes(f_sys2new.count_ALL[0].T1).CopyTo(byteArrayAfterStruct, 112);
            BitConverter.GetBytes(f_sys2new.count_ALL[0].T2).CopyTo(byteArrayAfterStruct, 118);
            BitConverter.GetBytes(f_sys2new.count_ALL[0].O31).CopyTo(byteArrayAfterStruct, 122);
            BitConverter.GetBytes(f_sys2new.count_ALL[0].O32).CopyTo(byteArrayAfterStruct, 126);
            BitConverter.GetBytes(f_sys2new.count_ALL[0].T31).CopyTo(byteArrayAfterStruct, 130);
            BitConverter.GetBytes(f_sys2new.count_ALL[0].T32).CopyTo(byteArrayAfterStruct, 134);

            //2
            BitConverter.GetBytes(f_sys2new.rmk[1].count.O1).CopyTo(byteArrayAfterStruct, 138);
            BitConverter.GetBytes(f_sys2new.rmk[1].count.O2).CopyTo(byteArrayAfterStruct, 139);
            BitConverter.GetBytes(f_sys2new.rmk[1].count.T1).CopyTo(byteArrayAfterStruct, 140);
            BitConverter.GetBytes(f_sys2new.rmk[1].count.T2).CopyTo(byteArrayAfterStruct, 142);
            BitConverter.GetBytes(f_sys2new.rmk[1].count.O31).CopyTo(byteArrayAfterStruct, 144);
            BitConverter.GetBytes(f_sys2new.rmk[1].count.O32).CopyTo(byteArrayAfterStruct, 145);
            BitConverter.GetBytes(f_sys2new.rmk[1].count.T31).CopyTo(byteArrayAfterStruct, 146);
            BitConverter.GetBytes(f_sys2new.rmk[1].count.T32).CopyTo(byteArrayAfterStruct, 148);

            BitConverter.GetBytes(f_sys2new.rmk[1].Sr_skz_d1).CopyTo(byteArrayAfterStruct, 150);
            BitConverter.GetBytes(f_sys2new.rmk[1].Sr_skz_d2).CopyTo(byteArrayAfterStruct, 154);
            BitConverter.GetBytes(f_sys2new.rmk[1].Sr_pik_d1).CopyTo(byteArrayAfterStruct, 158);
            BitConverter.GetBytes(f_sys2new.rmk[1].Sr_pik_d2).CopyTo(byteArrayAfterStruct, 162);

            BitConverter.GetBytes(f_sys2new.rmk[1].pred_skz_d1).CopyTo(byteArrayAfterStruct, 166);
            BitConverter.GetBytes(f_sys2new.rmk[1].pred_skz_d2).CopyTo(byteArrayAfterStruct, 170);
            BitConverter.GetBytes(f_sys2new.rmk[1].pred_pik_d1).CopyTo(byteArrayAfterStruct, 174);
            BitConverter.GetBytes(f_sys2new.rmk[1].pred_pik_d2).CopyTo(byteArrayAfterStruct, 178);

            BitConverter.GetBytes(f_sys2new.rmk[1].Sr_M_d1).CopyTo(byteArrayAfterStruct, 182);
            BitConverter.GetBytes(f_sys2new.rmk[1].Sr_M_d2).CopyTo(byteArrayAfterStruct, 186);

            BitConverter.GetBytes(f_sys2new.rmk[1].Cnt_Bad_red).CopyTo(byteArrayAfterStruct, 190);
            BitConverter.GetBytes(f_sys2new.rmk[1].Cnt_Bad_yel).CopyTo(byteArrayAfterStruct, 194);

            BitConverter.GetBytes(f_sys2new.rmk[1].Sum_skz_d1).CopyTo(byteArrayAfterStruct, 198);
            BitConverter.GetBytes(f_sys2new.rmk[1].Sum_pik_d1).CopyTo(byteArrayAfterStruct, 202);
            BitConverter.GetBytes(f_sys2new.rmk[1].Sum_skz_d2).CopyTo(byteArrayAfterStruct, 206);
            BitConverter.GetBytes(f_sys2new.rmk[1].Sum_pik_d2).CopyTo(byteArrayAfterStruct, 210);

            BitConverter.GetBytes(f_sys2new.rmk[1].Prozent_yel).CopyTo(byteArrayAfterStruct, 214);
            BitConverter.GetBytes(f_sys2new.rmk[1].Prozent_red).CopyTo(byteArrayAfterStruct, 218);

            BitConverter.GetBytes(f_sys2new.rmk[1].cnt_d1).CopyTo(byteArrayAfterStruct, 222);
            BitConverter.GetBytes(f_sys2new.rmk[1].cnt_d2).CopyTo(byteArrayAfterStruct, 224);

            BitConverter.GetBytes(f_sys2new.rmk[1].defect_file).CopyTo(byteArrayAfterStruct, 226);

            BitConverter.GetBytes(f_sys2new.count_ALL[1].O1).CopyTo(byteArrayAfterStruct, 228);
            BitConverter.GetBytes(f_sys2new.count_ALL[1].O2).CopyTo(byteArrayAfterStruct, 232);
            BitConverter.GetBytes(f_sys2new.count_ALL[1].T1).CopyTo(byteArrayAfterStruct, 236);
            BitConverter.GetBytes(f_sys2new.count_ALL[1].T2).CopyTo(byteArrayAfterStruct, 240);
            BitConverter.GetBytes(f_sys2new.count_ALL[1].O31).CopyTo(byteArrayAfterStruct, 244);
            BitConverter.GetBytes(f_sys2new.count_ALL[1].O32).CopyTo(byteArrayAfterStruct, 248);
            BitConverter.GetBytes(f_sys2new.count_ALL[1].T31).CopyTo(byteArrayAfterStruct, 252);
            BitConverter.GetBytes(f_sys2new.count_ALL[1].T32).CopyTo(byteArrayAfterStruct, 256);

            using (var stream = new System.IO.FileStream(path, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write))//запись в файл
            {
                //Перемещаемся в файле на 100 байт от начала
                //stream.Seek(0, System.IO.SeekOrigin.Begin);
                //Записываем буфер
                stream.Write(byteArrayAfterStruct, 0, byteArrayAfterStruct.Length - 8);
            }

        }
    }
}
