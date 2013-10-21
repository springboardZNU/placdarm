using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//test 1

namespace placdarm
{
    class Hod
    {
        //--------------- ВЫВОД ПОЛ ------------------------------------
        public static void Disp(char[,] M)
        {
            for (int iy = 0; iy < M.GetLength(0); iy++)
            {
                for (int k = 8 - iy; k >= 0; k--)
                {
                    Console.Write(" ");
                }
                for (int ix = 0; ix < M.GetLength(1); ix++)
                {
                    Console.Write(M[iy, ix] + " ");
                }
                Console.WriteLine();
            }
        }
        //------------------НОРМАЛИЗАЦИЯ МАССИВА ----------------------------
        private static void NormalMass(char[,] M) // функция нормализации массива
        {
            for (int iy = 0; iy < M.GetLength(0); iy++)
            {
                for (int jx = 0; jx < M.GetLength(1); jx++)
                {
                    if (M[iy, jx] == 'x') M[iy, jx] = '.';
                    else
                    {
                        if (M[iy, jx] == 'W') M[iy, jx] = 'w';
                        if (M[iy, jx] == 'B') M[iy, jx] = 'b';
                        if (M[iy, jx] == 'Q') M[iy, jx] = 'q';
                        if (M[iy, jx] == 'A') M[iy, jx] = 'q';
                        if (M[iy, jx] == 'a') M[iy, jx] = 'w';
                        if (M[iy, jx] == 'Z') M[iy, jx] = 'z';
                        if (M[iy, jx] == 'c') M[iy, jx] = 'b';
                        if (M[iy, jx] == 'C') M[iy, jx] = 'z';
                    }
                }
            }
        }
        //**************** КОНЕЦ * НОРМАЛИЗАЦИИ МАССИВА *********************
        private static void Add(char[,] M, int y, int x, char A)
        {
            if (PustoePole(M, y, x)) M[y, x] = A;
        }
        private static bool VMassive(char[,] M, int y, int x)
        {
            if (y > 8 || y < 0 || x > 9 || x < 0) return false;
            return true;
        }
        private static bool VPole(char[,] M, int y, int x)
        {
            if (!VMassive(M, y, x)) return false;
            if (y == 0) if (x > 5) return false;
            if (y == 1) if (x > 6) return false;
            if (y == 2) if (x > 7) return false;
            if (y == 3) if (x > 8) return false;
            if (y == 5) if (x < 1) return false;
            if (y == 6) if (x < 2) return false;
            if (y == 7) if (x < 3) return false;
            if (y == 8) if (x < 4) return false;
            return true;
        }
        private static bool PustoePole(char[,] M, int y, int x)
        {
            if (!VMassive(M, y, x)) return false;
            if (M[y, x] == '.' || M[y, x] == 'x') return true;
            return false;
        }
        //----------------ХОД ГЕНЕРАЛ--------------------
        private static void General(char[,] M, int y, int x)
        {
            // ХОД ГЕНЕРАЛА
            if (M[y, x] == 'q' || M[y, x] == 'Q' || M[y, x] == 'Z' || M[y, x] == 'z')
            {
                Add(M, y - 2, x, 'x'); //1
                Add(M, y, x + 2, 'x');   //2
                Add(M, y + 2, x + 2, 'x'); //3
                Add(M, y + 2, x, 'x'); //4
                Add(M, y, x - 2, 'x'); //5
                Add(M, y - 2, x - 2, 'x'); //6                
            }

            // БОЙ ГЕНЕРАЛА 
            if (ProvBoyGeneral(M, y, x, -1, -1)) VudelenieFiguruBoy(M, y - 1, x - 1); // проверяет ВОЗМОЖНОСТЬ ПОБИТЬ генералом и выделяет жертву
            if (ProvBoyGeneral(M, y, x, -1, 0)) VudelenieFiguruBoy(M, y - 1, x);
            if (ProvBoyGeneral(M, y, x, 0, 1)) VudelenieFiguruBoy(M, y, x + 1);
            if (ProvBoyGeneral(M, y, x, 1, 1)) VudelenieFiguruBoy(M, y + 1, x + 1);
            if (ProvBoyGeneral(M, y, x, 1, 0)) VudelenieFiguruBoy(M, y + 1, x);
            if (ProvBoyGeneral(M, y, x, 0, -1)) VudelenieFiguruBoy(M, y, x - 1);

        }
        //-----------------ПРОВЕРКА ВОЗМОЖНОСТИ ПОБИТЬ ГЕНЕРАЛУ------------------
        private static bool ProvBoyGeneral(char[,] M, int y, int x, int ky, int kx)  // kx, ky это коэфициэнт
        {
            if (('q' == M[y, x] && VPole(M, y + ky, x + kx) && M[y + ky, x + kx] == 'b') || ('z' == M[y, x] && VPole(M, y + ky, x + kx) && M[y + ky, x + kx] == 'w')) // фигура противоположного цвета и не генерал
            {
                int kolvoVokrug = KolvoElem(M, y + ky, x + kx);
                if (kolvoVokrug == 20 || kolvoVokrug == 1) return true; // фигура не больше двух, в проверке на тройку не нуждается               
            }

            return false;
        }
        //----------------ХОД через 3 клетки + ход клюшкой малой и большой --------------------
        private static void Thrid(char[,] M, int x, int y)
        {
            if (PustoePole(M, x - 1, y - 1))   // 1 - верх (проверка допустимых клеток)
            {
                if (PustoePole(M, x - 1, y - 2)) Add(M, x - 1, y - 3, 'x'); // ход клюшкой 2b
                if (PustoePole(M, x - 2, y - 2))
                {
                    Add(M, x - 2, y - 3, 'x'); // ход клюшкой 1a
                    if (PustoePole(M, x - 2, y - 3)) Add(M, x - 2, y - 4, 'x');      // через лево
                }
            }
            if (PustoePole(M, x, y - 1))
            {
                if (PustoePole(M, x - 1, y - 2)) Add(M, x - 2, y - 3, 'x'); // ход клюшкой 1b
                if (PustoePole(M, x, y - 2))
                {
                    Add(M, x - 1, y - 3, 'x'); // ход клюшкой 2a
                    if (PustoePole(M, x - 1, y - 3)) Add(M, x - 2, y - 4, 'x');      //через право
                }
            }
            //----------------------------------------------------------------
            if (PustoePole(M, x, y - 1))   // 2 - верх-право (проверка допустимых клеток) // через верх
            {
                if (PustoePole(M, x + 1, y - 1)) Add(M, x + 2, y - 1, 'x'); // ход клюшкой 4b
                if (PustoePole(M, x, y - 2))
                {
                    Add(M, x + 1, y - 2, 'x'); // ход клюшкой 3a
                    if (PustoePole(M, x + 1, y - 2)) Add(M, x + 2, y - 2, 'x');
                }
            }
            if (PustoePole(M, x + 1, y))  //через низ
            {
                if (PustoePole(M, x + 1, y - 1)) Add(M, x + 1, y - 2, 'x'); // ход клюшкой 3b
                if (PustoePole(M, x + 2, y))
                {
                    Add(M, x + 2, y - 1, 'x'); // ход клюшкой 4a
                    if (PustoePole(M, x + 2, y - 1)) Add(M, x + 2, y - 2, 'x');
                }
            }
            //----------------------------------------------------------------
            if (PustoePole(M, x + 1, y))   // 3 - низ-право (проверка допустимых клеток)
            {
                if (PustoePole(M, x + 2, y + 1)) Add(M, x + 3, y + 2, 'x'); // ход клюшкой 6b
                if (PustoePole(M, x + 2, y))
                {
                    Add(M, x + 3, y + 1, 'x'); // ход клюшкой 5a
                    if (PustoePole(M, x + 3, y + 1)) Add(M, x + 4, y + 2, 'x');      // через верх
                }
            }
            if (PustoePole(M, x + 1, y + 1))
            {
                if (PustoePole(M, x + 2, y + 1)) Add(M, x + 3, y + 1, 'x'); // ход клюшкой 5b
                if (PustoePole(M, x + 2, y + 2))
                {
                    Add(M, x + 3, y + 2, 'x'); // ход клюшкой 6a
                    if (PustoePole(M, x + 3, y + 2)) Add(M, x + 4, y + 2, 'x');     //через низ
                }
            }
            //----------------------------------------------------------------            
            if (PustoePole(M, x + 1, y + 1))   // 4 - низ(проверка допустимых клеток)
            {
                if (PustoePole(M, x + 1, y + 2)) Add(M, x + 1, y + 3, 'x'); // ход клюшкой 8b
                if (PustoePole(M, x + 2, y + 2))
                {
                    Add(M, x + 2, y + 3, 'x'); // ход клюшкой 7a
                    if (PustoePole(M, x + 2, y + 3)) Add(M, x + 2, y + 4, 'x');      // через право
                }
            }
            if (PustoePole(M, x, y + 1))
            {
                if (PustoePole(M, x + 1, y + 2)) Add(M, x + 2, y + 3, 'x'); // ход клюшкой 7b
                if (PustoePole(M, x, y + 2))
                {
                    Add(M, x + 1, y + 3, 'x');// ход клюшкой 8a
                    if (PustoePole(M, x + 1, y + 3)) Add(M, x + 2, y + 4, 'x');     //через лево
                }
            }
            //----------------------------------------------------------------            
            if (PustoePole(M, x, y + 1))  // 5 - низ-лево (проверка допустимых клеток)
            {
                if (PustoePole(M, x - 1, y + 1)) Add(M, x - 2, y + 1, 'x'); // ход клюшкой 10b 
                if (PustoePole(M, x, y + 2))
                {
                    Add(M, x - 1, y + 2, 'x');// ход клюшкой 9a
                    if (PustoePole(M, x - 1, y + 2)) Add(M, x - 2, y + 2, 'x');     //через верх
                }
            }
            if (PustoePole(M, x - 1, y))
            {
                if (PustoePole(M, x - 1, y + 1)) Add(M, x - 1, y + 2, 'x'); // ход клюшкой 9b
                if (PustoePole(M, x - 2, y))
                {
                    Add(M, x - 2, y + 1, 'x');// ход клюшкой 10a
                    if (PustoePole(M, x - 2, y + 1)) Add(M, x - 2, y + 2, 'x');      // через низ
                }
            }
            //--------------------------------------------------------------
            if (PustoePole(M, x - 1, y - 1))   // 6 - верх-лево (проверка допустимых клеток)
            {
                if (PustoePole(M, x - 2, y - 1)) Add(M, x - 3, y - 1, 'x'); // ход клюшкой 11b
                if (PustoePole(M, x - 2, y - 2))
                {
                    Add(M, x - 3, y - 1, 'x');// ход клюшкой 11a
                    if (PustoePole(M, x - 3, y - 2)) Add(M, x - 4, y - 2, 'x');      // через верх
                }
            }
            if (PustoePole(M, x - 1, y))
            {
                if (PustoePole(M, x - 2, y - 1)) Add(M, x - 3, y - 2, 'x'); // ход клюшкой 12b
                if (PustoePole(M, x - 2, y))
                {
                    Add(M, x - 3, y - 2, 'x');// ход клюшкой 12a
                    if (PustoePole(M, x - 3, y - 1)) Add(M, x - 4, y - 2, 'x');    //через низ
                }
            }
            //------------------------------------------------------------ 
        }
        //----------------ХОД ПО ГОРИЗОНТАЛИ И ПО КОСОЙ + ХОД УГЛОМ --------------------
        private static void Four(char[,] M, int y, int x)
        {
            int ix = x - 1, iy = y - 1;         // в левый верхний
            while (PustoePole(M, iy, ix))
            {
                Add(M, iy, ix, 'x');
                int zx = ix + 1, zy = iy - 1;       // ход углом  в верх-право
                while (PustoePole(M, zy, zx))
                {
                    Add(M, zy, zx, 'x');
                    zy--;
                    zx++;
                }
                zx = ix - 1; zy = iy + 1;       // ход углом  в низ-лево
                while (PustoePole(M, zy, zx))
                {
                    Add(M, zy, zx, 'x');
                    zy++;
                    zx--;
                }
                ix--;
                iy--;
            }
            ix = x - 2;                         // в левый верхний по косой
            iy = y - 1;
            while (PustoePole(M, iy, ix))
            {
                Add(M, iy, ix, 'x');
                int zx = ix, zy = iy - 1;       // ход углом  в верх-право по косой
                while (PustoePole(M, zy, zx))
                {
                    Add(M, zy, zx, 'x');
                    zy--;
                }
                zx = ix; zy = iy + 1;       // ход углом  в низ-лево по косой
                while (PustoePole(M, zy, zx))
                {
                    Add(M, zy, zx, 'x');
                    zy++;
                }
                ix -= 2;
                iy--;
            }
            ix = x + 1;                         // в правый нижний
            iy = y + 1;
            while (PustoePole(M, iy, ix))
            {
                Add(M, iy, ix, 'x');
                int zx = ix + 1, zy = iy - 1;       // ход углом  в верх-право
                while (PustoePole(M, zy, zx))
                {
                    Add(M, zy, zx, 'x');
                    zy--;
                    zx++;
                }
                zx = ix - 1; zy = iy + 1;       // ход углом  в низ-лево
                while (PustoePole(M, zy, zx))
                {
                    Add(M, zy, zx, 'x');
                    zy++;
                    zx--;
                }
                ix++;
                iy++;
            }
            ix = x - 1;                         // в левый нижний по косой
            iy = y + 1;
            while (PustoePole(M, iy, ix))
            {
                Add(M, iy, ix, 'x');
                int zx = ix - 1, zy = iy - 1;       // ход углом  в верх-лево по косой
                while (PustoePole(M, zy, zx))
                {
                    Add(M, zy, zx, 'x');
                    zy--;
                    zx--;
                }
                zx = ix + 1;
                zy = iy + 1;       // ход углом  в низ-лево по косой
                while (PustoePole(M, zy, zx))
                {
                    Add(M, zy, zx, 'x');
                    zy++;
                    zx++;
                }
                ix--;
                iy++;
            }
            ix = x;                             // в левый верхний
            iy = y - 1;
            while (PustoePole(M, iy, ix))
            {
                Add(M, iy, ix, 'x');
                int zx = ix - 2, zy = iy - 1;       // ход углом  в верх-лево по косой
                while (PustoePole(M, zy, zx))
                {
                    Add(M, zy, zx, 'x');
                    zy--;
                    zx -= 2;
                }
                zx = ix + 2;
                zy = iy + 1;       // ход углом  в низ-лево по косой
                while (PustoePole(M, zy, zx))
                {
                    Add(M, zy, zx, 'x');
                    zy++;
                    zx += 2;
                }

                iy--;
            }
            ix = x + 1;                         // в правый верхний по косой
            iy = y - 1;
            while (PustoePole(M, iy, ix))
            {
                Add(M, iy, ix, 'x');
                int zx = ix - 1, zy = iy - 1;       // ход углом  в верх-право
                while (PustoePole(M, zy, zx))
                {
                    Add(M, zy, zx, 'x');
                    zy--;
                    zx--;
                }
                zx = ix + 1; zy = iy + 1;       // ход углом  в низ-лево
                while (PustoePole(M, zy, zx))
                {
                    Add(M, zy, zx, 'x');
                    zy++;
                    zx++;
                }
                ix++;
                iy--;
            }
            ix = x;                             // в правый верхний
            iy = y + 1;
            while (PustoePole(M, iy, ix))
            {
                Add(M, iy, ix, 'x');
                int zx = ix - 2, zy = iy - 1;       // ход углом  в верх-лево по косой
                while (PustoePole(M, zy, zx))
                {
                    Add(M, zy, zx, 'x');
                    zy--;
                    zx -= 2;
                }
                zx = ix + 2;
                zy = iy + 1;       // ход углом  в низ-лево по косой
                while (PustoePole(M, zy, zx))
                {
                    Add(M, zy, zx, 'x');
                    zy++;
                    zx += 2;
                }
                iy++;
            }
            ix = x + 2;                         // в правый нижний по косой
            iy = y + 1;
            while (PustoePole(M, iy, ix))
            {
                Add(M, iy, ix, 'x');
                int zx = ix, zy = iy - 1;       // ход углом  в верх-право
                while (PustoePole(M, zy, zx))
                {
                    Add(M, zy, zx, 'x');
                    zy--;
                }
                zx = ix; zy = iy + 1;       // ход углом  в низ-лево
                while (PustoePole(M, zy, zx))
                {
                    Add(M, zy, zx, 'x');
                    zy++;
                }
                ix += 2;
                iy++;
            }
        }
        //------ПО ГОРИЗОНТАЛЯМ + ПОД ПРЯМЫМ УГЛОМ-----------------------------------
        private static void Fifth(char[,] M, int y, int x)
        {
            int ix = x - 1, iy = y;         // в лево
            while (PustoePole(M, iy, ix))
            {
                Add(M, iy, ix, 'x');
                int zx = ix - 1, zy = iy - 2;       // ход углом  в верх
                while (PustoePole(M, zy, zx))
                {
                    Add(M, zy, zx, 'x');
                    zy = zy - 2;
                    zx--;
                }
                zx = ix + 1; zy = iy + 2;       // ход углом  в низ
                while (PustoePole(M, zy, zx))
                {
                    Add(M, zy, zx, 'x');
                    zy = zy + 2;
                    zx++;
                }
                ix--;
            }
            ix = x + 1;                         // в право            
            iy = y;
            while (PustoePole(M, iy, ix))
            {
                Add(M, iy, ix, 'x');
                int zx = ix - 1, zy = iy - 2;     // ход углом  в верх
                while (PustoePole(M, zy, zx))
                {
                    Add(M, zy, zx, 'x');
                    zy = zy - 2;
                    zx--;
                }
                zx = ix + 1; zy = iy + 2;       // ход углом  в низ
                while (PustoePole(M, zy, zx))
                {
                    Add(M, zy, zx, 'x');
                    zy = zy + 2;
                    zx++;
                }
                ix++;
            }
            ix = x + 1;                           // в низ
            iy = y + 2;
            while (PustoePole(M, iy, ix))
            {
                Add(M, iy, ix, 'x');
                int zx = ix + 1, zy = iy;       // ход углом  в право 
                while (PustoePole(M, zy, zx))
                {
                    Add(M, zy, zx, 'x');
                    zx++;
                }
                zx = ix - 1; zy = iy;       // ход углом  в лево 
                while (PustoePole(M, zy, zx))
                {
                    Add(M, zy, zx, 'x');
                    zx--;
                }
                iy = iy + 2;
                ix++;
            }
            ix = x - 1;
            iy = y - 2;                         // в верх
            while (PustoePole(M, iy, ix))
            {
                Add(M, iy, ix, 'x');
                int zx = ix + 1, zy = iy;       // ход углом  в право 
                while (PustoePole(M, zy, zx))
                {
                    Add(M, zy, zx, 'x');
                    zx++;
                }
                zx = ix - 1; zy = iy;       // ход углом  в лево 
                while (PustoePole(M, zy, zx))
                {
                    Add(M, zy, zx, 'x');
                    zx--;
                }

                iy = iy - 2;
                ix--;
            }
        }
        //******** КОНЕЦ  ПО ГОРИЗОНТАЛЯМ + ПОД ПРЯМЫМ УГЛОМ *********************
        public static char[,] NewMass()
        {
            int x = 10, y = 9;//размер массива
            char[,] _M = new char[y, x];
            for (int iy = 0; iy < _M.GetLength(0); iy++)
            {
                for (int ix = 0; ix < _M.GetLength(1); ix++)
                {
                    _M[iy, ix] = '.';
                    if (!VPole(_M, iy, ix)) _M[iy, ix] = ' ';
                }
            }
            _M[0, 9] = '1';
            _M[0, 7] = 'n'; // x
            _M[0, 8] = 'n'; // y
            _M[1, 8] = '0'; // Преимущество белых 
            _M[1, 9] = '0'; // Преимущество черных
            return _M;
        }
        //------------------ВЫДЕЛЕНИЕ ФИГУРЫ ----------------------------
        private static void VudelenieFiguru(char[,] M, int y, int x)
        {
            if (M[y, x] == 'w') { M[y, x] = 'W'; return; }
            if (M[y, x] == 'b') { M[y, x] = 'B'; return; }
            if (M[y, x] == 'q') { M[y, x] = 'Q'; return; }
            if (M[y, x] == 'z') { M[y, x] = 'Z'; return; }
        }
        //-----------------ПРОВЕРКА ОДИНАКОВОСТИ ФИГУРЫ С УЧЕТОМ ГЕНЕРАЛА------------------
        private static bool ProvOdinakov(char[,] M, int x, int y, int kx, int ky)  // kx, ky это коэфициэнт
        {
            if (
                (VMassive(M, y + ky, x + kx)) &&
                ((M[y + ky, x + kx] == M[y, x]) ||                   //фигура одинаковая
                (M[y, x] == 'w' && M[y + ky, x + kx] == 'q') || //пешка и генерал белые
                (M[y, x] == 'q' && M[y + ky, x + kx] == 'w') || // генерал и пешка белые
                (M[y, x] == 'b' && M[y + ky, x + kx] == 'z') || //пешка и генерал черные
                (M[y, x] == 'z' && M[y + ky, x + kx] == 'b'))   // генерал и пешка черные
                ) return true;
            else return false;
        }
        //-----------------ПОИСК ТРОЙКИ --------------------------------
        public static bool Troyka(char[,] M, int y, int x)
        {
            int kolvo = 0; //количество элементов вокруг выбраной клетки

            //массив элементов частей тройки, размер 6(максимально возможное колво) на 2(координаты) 
            int[,] mas_temp = { { -1, -1 }, { -1, -1 }, { -1, -1 }, { -1, -1 }, { -1, -1 }, { -1, -1 }, { -1, -1 } };
            mas_temp[kolvo, 0] = x; mas_temp[kolvo, 1] = y; //сохранение координат первого элемента
            // сохраняем координаты остальных элементов, которые окружают его

            if (ProvOdinakov(M, x, y, -1, -1)) { kolvo++; mas_temp[kolvo, 0] = x - 1; mas_temp[kolvo, 1] = y - 1; }
            if (ProvOdinakov(M, x, y, 0, -1)) { kolvo++; mas_temp[kolvo, 0] = x; mas_temp[kolvo, 1] = y - 1; }
            if (ProvOdinakov(M, x, y, 1, 0)) { kolvo++; mas_temp[kolvo, 0] = x + 1; mas_temp[kolvo, 1] = y; }
            if (ProvOdinakov(M, x, y, 1, 1)) { kolvo++; mas_temp[kolvo, 0] = x + 1; mas_temp[kolvo, 1] = y + 1; }
            if (ProvOdinakov(M, x, y, 0, 1)) { kolvo++; mas_temp[kolvo, 0] = x; mas_temp[kolvo, 1] = y + 1; }
            if (ProvOdinakov(M, x, y, -1, 0)) { kolvo++; mas_temp[kolvo, 0] = x - 1; mas_temp[kolvo, 1] = y; }

            if (kolvo == 0 || kolvo > 2) return false; // не тройка, кличество фишек рядом не подходит
            if (mas_temp[2, 0] == -1) // третий элемент не заполнен, находим его и заполняем
            {
                //берем второй элемент тройки и проверяем что вокруг
                kolvo = 0; //количество элементов вокруг выбраной клетки

                x = mas_temp[1, 0]; // заносим в Х и У кординаты второго элемента
                y = mas_temp[1, 1];

                // сохраняем координаты остальных элементов, которые окружают его, проверяя не одинаковый элемент с первой ячейкой
                if (ProvOdinakov(M, x, y, -1, -1))
                {
                    kolvo++;
                    if (!(mas_temp[0, 0] == x - 1 && mas_temp[0, 1] == y - 1)) //если координаты не совпадают с первым элементом
                    { mas_temp[2, 0] = x - 1; mas_temp[2, 1] = y - 1; }
                }
                if (ProvOdinakov(M, x, y, 0, -1))
                {
                    kolvo++;
                    if (!(mas_temp[0, 0] == x && mas_temp[0, 1] == y - 1))
                    { mas_temp[2, 0] = x; mas_temp[2, 1] = y - 1; }
                }
                if (ProvOdinakov(M, x, y, 1, 0))
                {
                    kolvo++;
                    if (!(mas_temp[0, 0] == x + 1 && mas_temp[0, 1] == y))
                    { mas_temp[2, 0] = x + 1; mas_temp[2, 1] = y; }
                }
                if (ProvOdinakov(M, x, y, 1, 1))
                {
                    kolvo++;
                    if (!(mas_temp[0, 0] == x + 1 && mas_temp[0, 1] == y + 1))
                    { mas_temp[2, 0] = x + 1; mas_temp[2, 1] = y + 1; }
                }
                if (ProvOdinakov(M, x, y, 0, 1))
                {
                    kolvo++;
                    if (!(mas_temp[0, 0] == x && mas_temp[0, 1] == y + 1))
                    { mas_temp[2, 0] = x; mas_temp[2, 1] = y + 1; }
                }
                if (ProvOdinakov(M, x, y, -1, 0))
                {
                    kolvo++;
                    if (!(mas_temp[0, 0] == x - 1 && mas_temp[0, 1] == y))
                    { mas_temp[2, 0] = x - 1; mas_temp[2, 1] = y; }
                }

                if (kolvo == 0 || kolvo > 2) return false; //  не тройка, вокруг больше или меньше элементов                               
            }
            if (mas_temp[2, 0] != -1)
            {
                // проводим проверку остальных частей тройки что бы не было множества
                int proverka_na_mnozhestvo = 0;
                proverka_na_mnozhestvo = proverka_na_mnozhestvo + KolvoElem(M, mas_temp[0, 0], mas_temp[0, 1]);
                proverka_na_mnozhestvo = proverka_na_mnozhestvo + KolvoElem(M, mas_temp[1, 0], mas_temp[1, 1]);
                proverka_na_mnozhestvo = proverka_na_mnozhestvo + KolvoElem(M, mas_temp[2, 0], mas_temp[2, 1]);

                if (proverka_na_mnozhestvo != 4) return false; //скопление 
                if (    // проверяем на нелинейность
                    (mas_temp[0, 1] == mas_temp[1, 1] && mas_temp[1, 1] == mas_temp[2, 1]) ||//!=Y1==Y2==Y3
                    (mas_temp[0, 0] == mas_temp[1, 0] && mas_temp[1, 0] == mas_temp[2, 0]) //!=X1==X2==X3
                    ) return false; // тройная линия
                else
                {

                    int[] X = { mas_temp[0, 0], mas_temp[1, 0], mas_temp[2, 0] };
                    for (int j = 0; j < 2; j++)
                    {
                        for (int i = 0; i < 2; i++)//сортировка занчений Х по возрастанию для проверки на линейность Х
                        { if (X[i] > X[i + 1]) Swap(X, i); }
                    }
                    if (X[0] + 1 == X[1] && X[1] == X[2] - 1)   // тройная линия 
                    {
                        if (mas_temp[0, 1] == mas_temp[1, 1] || mas_temp[2, 1] == mas_temp[1, 1] || mas_temp[0, 1] == mas_temp[2, 1])
                        { return true; }
                        else return false;
                    }
                    else { return true; } //-----НАЙДЕНА ТРОЙКА----------
                }

                //return true;
            }

            return false;
        }
        //-------------SWAP------------------------------------------------------------
        private static void Swap(int[] X, int i)
        {
            int temp = X[i];
            X[i] = X[i + 1];
            X[i + 1] = temp;
        }
        //----------ВОЗМОЖНЫЕ ХОДЫ ТРОЙКИ---------------------------------------------
        private static void MovTroyka(char[,] M, int y, int x)
        {
            BoyTroyka(M, y, x, 0, -1); // в лево
            BoyTroyka(M, y, x, 0, 1); // в право
            BoyTroyka(M, y, x, -1, -1); // в левый верхний
            BoyTroyka(M, y, x, -1, 0);  // в правый верхний
            BoyTroyka(M, y, x, 1, 0); // в левый нижний
            BoyTroyka(M, y, x, 1, 1);  // в правый нижний            
        }
        private static void BoyTroyka(char[,] M, int y, int x, int ky, int kx)
        {
            int kx_protiv = 0, ky_protiv = 0; //противоположный коэфициэнт направления, для определения сильного звена

            if (kx == -1) kx_protiv = 1;
            else if (kx == 1) kx_protiv = -1;

            if (ky == -1) ky_protiv = 1;
            else if (ky == 1) ky_protiv = -1;

            int ix = x + kx, iy = y + ky;
            while (PustoePole(M, iy, ix))
            {
                ix = ix + kx;
                iy = iy + ky;
            }
            if (Vrag(M, y, x, iy, ix))
            {                           // проверка на самовостановление
                if (!Troyka(M, iy, ix)) VudelenieFiguruBoy(M, iy, ix); //если бьет не тройку
                else
                {
                    if (!ProvOdinakov(M, ix, iy, kx, ky))     // если бьет не сильное звено
                    {
                        if (ProvOdinakov(M, x, y, kx_protiv, ky_protiv))      // если бьет сильным звеном  
                        { VudelenieFiguruBoy(M, iy, ix); }
                    }
                }
            }
        }
        //-----------------Проверка клетки на наличие соперника !!! не хватает проверки на сильное звено--------------------------------------------
        private static bool Vrag(char[,] M, int y, int x, int iy, int ix)
        {
            if (!VMassive(M, iy, ix)) return false; //проверка выхода за границы
            if (M[y, x] == 'w' || M[y, x] == 'q')   //если белые фигуры, то, соперника фигуры будут такими:
            {
                if (M[iy, ix] == 'b' || M[iy, ix] == 'z') return true;
            }
            if (M[y, x] == 'b' || M[y, x] == 'z')
            {
                if (M[iy, ix] == 'w' || M[iy, ix] == 'q') return true;
            }
            return false; // напротив находится фигура не соперника
        }
        //-----------------Выделение соперника под боем--------------------------------------------
        private static void VudelenieFiguruBoy(char[,] M, int y, int x)
        {
            if (M[y, x] == 'w') M[y, x] = 'a';
            if (M[y, x] == 'q') M[y, x] = 'A';
            if (M[y, x] == 'b') M[y, x] = 'c';
            if (M[y, x] == 'z') M[y, x] = 'C';
        }
        //----------------!!!!!!-ВСОМОГАТЕЛЬНАЯ ФУНКЦИЯ ТРОЙКИ ПОДСЧЕТА КОЛ-ВО ОКРУЖАЮЩИХ ЭЛЕМЕНТОВ---------------
        private static int KolvoElem(char[,] M, int x, int y)
        {
            int kolvo = 0;
            if (ProvOdinakov(M, x, y, -1, -1)) kolvo++;
            if (ProvOdinakov(M, x, y, 0, -1)) kolvo++;
            if (ProvOdinakov(M, x, y, 1, 0)) kolvo++;
            if (ProvOdinakov(M, x, y, 1, 1)) kolvo++;
            if (ProvOdinakov(M, x, y, 0, 1)) kolvo++;
            if (ProvOdinakov(M, x, y, -1, 0)) kolvo++;

            if (kolvo > 2) kolvo = 10;  // проверка на кучу, если куча, то вводится недопустимое число
            if (kolvo < 1) kolvo = 20;  // если ноль, то вводится недопустимое число равное 20 для хода генерала
            return kolvo;
        }

        //-----------------РАСЧЕТ ВОЗМОЖНЫХ ХОДОВ --------------------------------
        private static void RaschetHoda(char[,] M, int y, int x)
        {
            Thrid(M, y, x);    // ЧЕРЕЗ ТРИ КЛЕТКИ + КЛЮШКОЙ 
            Four(M, y, x);    // ПО ДИАГОНАЛЯМ + КОСОЙ + УГЛОМ
            Fifth(M, y, x);   // ПО ГОРИЗОНТАЛЯМ + УГЛОМ
            General(M, y, x);   // ХОД ГЕНЕРАЛОМ + БОЙ ГЕНЕРАЛОМ
        }
        public static String Click(char[,] M, int y, int x)
        {

            if (            // если нажато на не активную фигуру выполняет выделение и расчет хода
                (M[0, 9] == '0' && (M[y, x] == 'w' || M[y, x] == 'q')) ||  //если ход белых
                (M[0, 9] == '1' && (M[y, x] == 'b' || M[y, x] == 'z'))  //если ход черных  
                )
            {
                M[0, 7] = (char)(x + 48); //запись координат активной фигуры Х
                M[0, 8] = (char)(y + 48); //запись координат активной фигуры У
                NormalMass(M); // функция нормализации массива
                if (Troyka(M, y, x)) MovTroyka(M, y, x);
                RaschetHoda(M, y, x);
                VudelenieFiguru(M, y, x);// функция выделения фигуры ДОЛЖНА ВЫПОЛНЯТЬСЯ ПОСЛЕДНЕЙ !!!
            }
            else
            {
                if (M[y, x] == 'x' || M[y, x] == 'a' || M[y, x] == 'A' || M[y, x] == 'c' || M[y, x] == 'C') //нажатие на возможное место для хода
                {
                    //--------счетчик на победу в пять очков
                    if (M[y, x] != 'x' && M[0, 9] == '0') //бой белых + проверка боя (не обычного хода)
                    {
                        if (M[1, 9] == '0') //черные не лидируют
                        {
                            M[1, 8] = (char)(Convert.ToInt32(M[1, 8]) + 1); //добавляем 1 к лидерству белых
                        }
                        else  //черные лидируют
                        {
                            int raznica_fishek = (Convert.ToInt32(M[1, 9]) - 48); //отнимаем 1 от лидерства черных
                            raznica_fishek--;
                            M[1, 9] = (char)(raznica_fishek + 48);
                        }
                    }
                    else
                    {
                        if (M[y, x] != 'x' && M[0, 9] == '1') //бой черных  + проверка боя (не обычного хода)
                        {
                            if (M[1, 8] == '0') //белые не лидируют
                            {
                                M[1, 9] = (char)(Convert.ToInt32(M[1, 9]) + 1); //добавляем 1 к лидерству черных
                            }
                            else  //белые лидируют
                            {
                                int raznica_fishek = (Convert.ToInt32(M[1, 8]) - 48); //отнимаем 1 от лидерства белых
                                raznica_fishek--;
                                M[1, 8] = (char)(raznica_fishek + 48);
                            }
                        }
                    }
                    //-----------------------------------------------

                    char tmp = M[Convert.ToInt32(M[0, 8]) - 48, Convert.ToInt32(M[0, 7] - 48)];
                    M[y, x] = tmp; //замена побитой фишки или становление ее на пустое место
                    M[Convert.ToInt32(M[0, 8]) - 48, Convert.ToInt32(M[0, 7] - 48)] = '.'; //на место походившей устанавливаем пустое поле
                    //Console.WriteLine("M[" + (Convert.ToInt32(M[0, 8]) - 48)+","+(Convert.ToInt32(M[0, 7]) - 48)+"], tmp = "+tmp);
                    if (M[0, 9] == '1') M[0, 9] = '0'; //смена флага хода на противоположный
                    else M[0, 9] = '1';

                    M[0, 7] = 'n'; //обнуление координат активной фигуры Х, У
                    M[0, 8] = 'n';

                    NormalMass(M);
                }
            }
            //------------ПРОВЕРКА ПОБЕДЫ--------------------------
            if (M[1, 3] == 'q')
            {
                if (M[0, 3] == 'w') if (M[0, 2] == 'w') return "Захват поля, победа белых (2,5 очка)"; ;
            }
            else if (M[7, 6] == 'z')
            {
                if (M[8, 6] == 'b') if (M[8, 7] == 'b') return "Захват поля, победа черных (2,5 очка)"; ;
            }
            if (M[1, 8] == '5') return "Преимущество в пять фигур, победа белых (2 очка)";
            if (M[1, 9] == '5') return "Преимущество в пять фигур, победа черных (2 очка)";

            return "0";



            //проверка на победу!

        }
    }
}


// решить баги: 

//  ПРОВЕРКА ХОДА
//  1. если в тройке генерал                    - готово
//  2. бой генерала                             - готово
//  3. удар по тройке, сильное/слабое звенья    - готово
//          решить проблемму выхода за массив   - готово
//  4. самовостановление
//  5. четверки, самовостановление

//  ХОД
//  6. перестановка фигур                       - готово
//  7. проверки победы: занятое поле, убитый генерал, пять очек

//  8. СМЕНА ФЛАГА ХОДА                         - готово