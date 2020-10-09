CC   = g++
OPTS = -W -O3

#DEBUG=-g

SRCDIR= ../Cpp
OBJDIR= obj
INCDIR= ../Header
BINDIR= ../../EXE

SRCS= $(wildcard $(SRCDIR)/*.cpp)

INCLUDE = $(addprefix -I ,$(INCDIR))
OBJS = ${SRCS:$(SRCDIR)/%.cpp=$(OBJDIR)/%.o}
CFLAGS   = $(OPTS) $(INCLUDE) $(DEBUG)

TARGET = $(BINDIR)/nlc

all: build $(TARGET)



$(OBJS): $(OBJDIR)/%.o : $(SRCDIR)/%.cpp
	$(CC) $(CFLAGS) -c $< -o $@


$(TARGET): $(OBJS)
	${CC} ${CFLAGS} -o $@ $(OBJS)

.PHONY: all build clean 

build:
	@mkdir -p $(BINDIR)
	@mkdir -p $(OBJDIR)

clean:
	-@rm -f $(OBJDIR)

  
