#pragma once

#include "Bank.h"

namespace Management {
  //стадии игры, описанные в Этюдах.
  enum State {
    FIXED_COSTS = 0, //уплата налогов, постоянные издержки.
    UPDATE_DEMAND_OFFER, //обновление ситуации на рынке.
    REQUESTS_MATERIALS, //заявки на сырье и материалы.
    PRODUCTION, //производство продукции.
    SELLING, //продажа продукции.
    PAYOUT_LOAN_INTEREST, //выплата ссудного процента.
    REPAYMENT_LOAN, //погашение ссуды.
    GETTING_LOAN, //получение ссуды.
    REQUESTS_BUILDING, //заявки на строительство.
    NUMBER_OF_STATES, //кол-во стадий.
  };

  class Management {
  private:
    State state;
    Bank* _bank;

  public:

    Management();

    Management(Management &) = delete;

    void operator=(Management &) = delete;

    ~Management();

    void nextState(); //переход на следующую стадию игры.

    State getState() const;

    const Bank* getBank() const;
  };
}