library(ggplot2)

length_data <- read.csv("Single_Coefficient_Performance_LengthIncrement.csv")

length_data <- sapply(length_data[,1:3], as.numeric)

ggplot(length_data) +
  geom_line(aes(Longitud, Tiempo)) +
  labs(title = "Execution time per rule length")

varied_base_data <- read.csv("Single_Varied_Performance_BaseIncrement.csv")

varied_base_data[1:2] <- sapply(varied_base_data[,1:2], as.numeric)

ggplot(varied_base_data) +
  geom_line(aes(Base, Tiempo)) +
  labs(title = "Execution time per base")

varied_divisor_data <- read.csv("Single_Varied_Performance_DivisorIncrement.csv")

varied_divisor_data[1:2] <- sapply(varied_divisor_data[,1:2], as.numeric)

ggplot(varied_divisor_data) +
  geom_line(aes(Divisor, Tiempo)) +
  labs(title = "Execution time per divisor")
