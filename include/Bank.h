#ifndef MANAGEMENT_BANK_H
#define MANAGEMENT_BANK_H

#include <vector>

namespace Management {

  class Bank {
  public:
    struct DemandOffer {
      int UMat; //ед. сырья на игрока.
      int MinPrice; //минимальная цена покупки сырья.
      int UProd; //ед. продукции на игрока.
      int MaxPrice; //максимальная цела продажи продукции.
    };

  private:
    const std::vector<std::vector<double>> probOfPriceChange; //вероятности изменения уровня цен.
    const std::vector<DemandOffer> priceLevelInfo; //инофрмация по уровням цен.
    int price_level; //текущий уровень цен.

  public:
    Bank();

    ~Bank();

    int getPriceLevel() const;

    void setNewPriceLevel();

    DemandOffer getInfo() const;

    friend class Management;
  };
}


#endif
